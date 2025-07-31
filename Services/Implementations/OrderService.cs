using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Core.Entities.Enums;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartService _cartService;
        private readonly ICartRepository _cartRepository;
        private readonly ICouponService _couponService;
        private readonly IPaymentService _paymentService;
        private readonly IUserService _userService;

        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork,
            ICartService cartService, ICartRepository cartRepository, ICouponService couponService, IPaymentService paymentService, IUserService userService)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _cartRepository = cartRepository;
            _couponService = couponService;
            _paymentService = paymentService;
            _userService = userService;
        }

        public async Task<BaseResponse<OrderDto>> CreateOrderAsync(CreateOrderRequestModel model)
        {
            try
            {
                var currentUser = await _userService.GetCurrentUserAsync();
                if (currentUser == null)
                {
                    return new BaseResponse<OrderDto>
                    {
                        Message = "User not authenticated",
                        Status = false,
                        Data = null
                    };
                }

                var cart = await _cartRepository.GetCartAsync(c => c.CustomerId == currentUser.Id);

                if (cart is null)
                {
                    return new BaseResponse<OrderDto>
                    {
                        Message = "Cart not found or is empty",
                        Status = false,
                        Data = null
                    };
                }


                var order = new Order
                {
                    Name = $"ORDER-{DateTime.UtcNow:yyyy-MM-dd-HH-mm}",
                    OrderDate = DateTime.UtcNow,
                    Status = OrderEnum.Pending,
                    CustomerId = currentUser.Id,
                    OrderItems = new List<OrderItem>(),
                    Coupons = new List<Coupon>()
                };

                decimal totalPrice = 0;

                foreach (var cartItem in cart.CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        ProductName = cartItem.ProductName ,
                        PriceAtPurchase = cartItem.PricePerUnit ,
                        Quantity = cartItem.Quantity,
                        Order = order
                    };
                    order.OrderItems.Add(orderItem);
                    totalPrice += orderItem.PriceAtPurchase * orderItem.Quantity;
                }

                if (cart.Coupons != null && cart.Coupons.Any())
                {
                    foreach (var coupon in cart.Coupons)
                    {
                        var couponResult = await _couponService.RedeemCouponAsync(coupon.Code);
                        if (!couponResult.Status || couponResult.Data == null)
                        {
                            return new BaseResponse<OrderDto>
                            {
                                Message = $"Coupon '{coupon.Code}' could not be redeemed: {couponResult.Message}",
                                Status = false,
                                Data = null
                            };
                        }

                        order.Coupons.Add(couponResult.Data);
                    }
                }

                order.TotalPrice = totalPrice;

                await _orderRepository.CreateAsync(order);
                await _unitOfWork.SaveChangesAsync();

                cart.CartItems.Clear();
                cart.Coupons.Clear();
                await _cartRepository.DeleteAsync(cart);
                await _unitOfWork.SaveChangesAsync();

                var orderDto = new OrderDto
                {
                    Id = order.Id,
                    Name = order.Name,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice,
                    CustomerId = order.CustomerId,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.ProductName,
                        PriceAtPurchase = oi.PriceAtPurchase,
                        Quantity = oi.Quantity
                    }).ToList(),
                    Coupons = order.Coupons.Select(c => new CouponDto
                    {
                        Id = c.Id,
                        Code = c.Code,
                        DiscountPercentage = c.DiscountPercentage,
                        ValidFrom = c.ValidFrom,
                        ValidUntil = c.ValidUntil,
                        Status = c.Status
                    }).ToList()
                };

                var paymentResult = await _paymentService.InitiatePaymentAsync(order);
                if (!paymentResult.Status)
                {
                    order.Status = OrderEnum.Failed;
                    await _unitOfWork.SaveChangesAsync();

                    return new BaseResponse<OrderDto>
                    {
                        Message = "Payment initialization failed",
                        Status = false,
                        Data = null
                    };
                }

                orderDto.PaymentUrl = paymentResult.Data;
                return new BaseResponse<OrderDto>
                {
                    Message = "Order created successfully and payment initiated",
                    Status = true,
                    Data = orderDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> CancelOrderAsync(Guid orderId)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = "User not authenticated",
                    Data = false
                };
            }

            var order = await _orderRepository.GetOrderAsync(o => o.Id == orderId && o.CustomerId == currentUser.Id);
            if (order == null)
            {
                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = "Order not found or unauthorized",
                    Data = false
                };
            }

            if (order.Status == OrderEnum.Shipped || order.Status == OrderEnum.Delivered)
            {
                return new BaseResponse<bool>
                {
                    Status = false,
                    Message = "Cannot cancel order after shipping or delivery",
                    Data = false
                };
            }

            order.Status = OrderEnum.Cancelled;
            await _unitOfWork.SaveChangesAsync();

            return new BaseResponse<bool>
            {
                Status = true,
                Message = "Order cancelled successfully",
                Data = true
            };
        }

        public async Task<ICollection<BaseResponse<OrderDto>>> GetAllOrdersAsync()
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new List<BaseResponse<OrderDto>>();
            }

            var orders = await _orderRepository.GetAllAsync(o => o.CustomerId == currentUser.Id);
            return orders.Select(order => new BaseResponse<OrderDto>
            {
                Message = "Order retrieved successfully",
                Status = true,
                Data = new OrderDto
                {
                    Id = order.Id,
                    Name = order.Name,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice,
                    CustomerId = order.CustomerId,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.ProductName,
                        PriceAtPurchase = oi.PriceAtPurchase,
                        Quantity = oi.Quantity
                    }).ToList(),
                    Coupons = order.Coupons.Select(c => new CouponDto
                    {
                        Code = c.Code,
                        DiscountPercentage = c.DiscountPercentage,
                        ValidFrom = c.ValidFrom,
                        ValidUntil = c.ValidUntil,
                        Status = c.Status
                    }).ToList()
                }
            }).ToList();
        }

        public async Task<BaseResponse<OrderDto>> GetOrderByIdAsync(Guid id)
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new BaseResponse<OrderDto>
                {
                    Message = "User not authenticated",
                    Status = false,
                    Data = null
                };
            }

            var order = await _orderRepository.GetOrderAsync(o => o.Id == id && o.CustomerId == currentUser.Id);
            if (order == null)
            {
                return new BaseResponse<OrderDto>
                {
                    Message = "Order not found",
                    Status = false,
                    Data = null
                };
            }

            return new BaseResponse<OrderDto>
            {
                Message = "Order retrieved successfully",
                Status = true,
                Data = new OrderDto
                {
                    Id = order.Id,
                    Name = order.Name,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice,
                    CustomerId = order.CustomerId,
                    OrderItems = order.OrderItems.Select(oi => new OrderItemDto
                    {
                        ProductId = oi.ProductId,
                        ProductName = oi.ProductName,
                        PriceAtPurchase = oi.PriceAtPurchase,
                        Quantity = oi.Quantity
                    }).ToList(),
                    Coupons = order.Coupons.Select(c => new CouponDto
                    {
                        Code = c.Code,
                        DiscountPercentage = c.DiscountPercentage,
                        ValidFrom = c.ValidFrom,
                        ValidUntil = c.ValidUntil,
                        Status = c.Status
                    }).ToList()
                }
            };
        }
    }
}
