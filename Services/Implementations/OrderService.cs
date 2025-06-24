﻿﻿﻿using E_commerce.Core.Dtos;
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
       

        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork,
            ICartService cartService, ICartRepository cartRepository, ICouponService couponService)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _cartService = cartService;
            _cartRepository = cartRepository;
            _couponService = couponService;
        }

        public async Task<BaseResponse<OrderDto>> CreateOrderAsync(Guid customerId)
        {
            try
            {
                // Fetch customer's cart with items and products
               var cart = await _cartService.GetCartByCustomerIdAsync(customerId);

                if (cart == null)
                {
                    return new BaseResponse<OrderDto>
                    {
                        Message = "Cart not found for customer",
                        Status = false,
                        Data = null
                    };
                }

                if (cart.CartItems == null || !cart.CartItems.Any())
                {
                    return new BaseResponse<OrderDto>
                    {
                        Message = "Cart is empty",
                        Status = false,
                        Data = null
                    };
                }

                // Create order entity
                var order = new Order
                {
                    Name = $"ORDER-{DateTime.UtcNow:yyyy-MM-dd-HH-mm}",
                    OrderDate = DateTime.UtcNow,
                    Status = OrderEnum.Pending,
                    CustomerId = customerId,
                    OrderItems = new List<OrderItem>(),
                    Coupons = new List<Coupon>()
                };

                decimal totalPrice = 0;

                // Snapshot cart items to order items
                foreach (var cartItem in cart.CartItems)
                {
                    var orderItem = new OrderItem
                    {
                        ProductId = cartItem.ProductId,
                        ProductName = cartItem.Product?.Name ?? "Unknown",
                        PriceAtPurchase = cartItem.Product?.Price ?? 0,
                        Quantity = cartItem.Quantity,
                        Order = order
                    };
                    order.OrderItems.Add(orderItem);
                    totalPrice += orderItem.PriceAtPurchase * orderItem.Quantity;
                }

                // Apply coupons if any
                
                if (cart.Coupons != null && cart.Coupons.Any())
                {
                    foreach (var coupon in cart.Coupons)
                    {
                        order.Coupons.Add(coupon);
                    }
                }

                order.TotalPrice = totalPrice;

                // Save order
                await _orderRepository.CreateAsync(order);
                await _unitOfWork.SaveChangesAsync();

                // Clear cart items and coupons but keep cart entity
                cart.CartItems.Clear();
                cart.Coupons.Clear();
                await _cartRepository.Update(cart);
                await _unitOfWork.SaveChangesAsync();

                // Prepare response DTO
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
                        DiscountAmount = c.DiscountAmount
                    }).ToList()
                };

                // Initiate payment (placeholder)
                await InitiatePaymentAsync(order);

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

        public async Task<ICollection<BaseResponse<OrderDto>>> GetAllOrdersByCustomerIdAsync(Guid customerId)
        {
            var orders = await _orderRepository.GetAllAsync(o => o.CustomerId == customerId);
            var responseList = new List<BaseResponse<OrderDto>>();

            foreach (var order in orders)
            {
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
                        Code = c.Code,
                        DiscountPercentage = c.DiscountPercentage,
                        ValidFrom = c.ValidFrom,
                        ValidUntil = c.ValidUntil,
                        Status = c.Status
                    }).ToList()
                };

                responseList.Add(new BaseResponse<OrderDto>
                {
                    Message = "Order retrieved successfully",
                    Status = true,
                    Data = orderDto
                });
            }

            return responseList;
        }

        public async Task<BaseResponse<OrderDto>> GetOrderByIdAsync(Guid id)
        {
            var order = await _orderRepository.GetAsync(o => o.Id == id);
            if (order == null)
            {
                return new BaseResponse<OrderDto>
                {
                    Message = "Order not found",
                    Status = false,
                    Data = null
                };
            }

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
                    Code = c.Code,
                    DiscountPercentage = c.DiscountPercentage,
                    ValidFrom = c.ValidFrom,
                    ValidUntil = c.ValidUntil,
                    Status = c.Status
                }).ToList()
            };

            return new BaseResponse<OrderDto>
            {
                Message = "Order retrieved successfully",
                Status = true,
                Data = orderDto
            };
        }
    }
}
