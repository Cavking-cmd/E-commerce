using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Services.Implementations
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly ICartItemRepository _cartItemRepository;

        public OrderItemService(IOrderItemRepository orderItemRepository, ICartItemRepository cartItemRepository)
        {
            _orderItemRepository = orderItemRepository;
            _cartItemRepository = cartItemRepository;
        }

        public async Task<BaseResponse<OrderItemDto>> CreateOrderItemAsync(CreateOrderItemRequestModel model)
        {
            try
            {
                // Convert cart items to order items for the given order id
                var cartItems = await _cartItemRepository.GetCartItemsByUserIdAsync(model.UserId);
                if (cartItems == null || !cartItems.Any())
                {
                    return new BaseResponse<OrderItemDto>
                    {
                        Message = "No cart items found for the user.",
                        Status = false,
                        Data = null
                    };
                }

                var orderItems = cartItems.Select(ci => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = model.OrderId,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    //PriceAtPurchase = ci.PriceAtPurchase,
                    //CreatedAt = DateTime.UtcNow,
                    //UpdatedAt = DateTime.UtcNow
                }).ToList();

                foreach (var orderItem in orderItems)
                {
                    await _orderItemRepository.CreateAsync(orderItem);
                }

               //clear cart items after order creation
                foreach (var cartItem in cartItems)
                {
                    await _cartItemRepository.ClearCartAsync(cartItem.CartId);
                }

                // Return the first created order item as a sample response
                var firstOrderItem = orderItems.First();

                var orderItemDto = new OrderItemDto
                {
                    //Id = firstOrderItem.Id,
                    OrderId = firstOrderItem.OrderId,
                    ProductId = firstOrderItem.ProductId,
                    Quantity = firstOrderItem.Quantity,
                    PriceAtPurchase = firstOrderItem.PriceAtPurchase
                };

                return new BaseResponse<OrderItemDto>
                {
                    Message = "Order items created successfully.",
                    Status = true,
                    Data = orderItemDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderItemDto>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteOrderItem(Guid id)
        {
            try
            {
                var orderItem = await _orderItemRepository.GetOrderByIdAsync(id);
                if (orderItem == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "Order item not found.",
                        Status = false,
                        Data = false
                    };
                }

                await _orderItemRepository.SoftDeleteAsync(orderItem);

                return new BaseResponse<bool>
                {
                    Message = "Order item deleted successfully.",
                    Status = true,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<ICollection<OrderItemDto>>> GetAll()
        {
            try
            {
                var orderItems = await _orderItemRepository.GetAllOrdersAsync();
                var orderItemDtos = orderItems.Select(oi => new OrderItemDto
                {
                    OrderId = oi.OrderId,
                    ProductId = oi.ProductId,
                    Quantity = oi.Quantity,
                    PriceAtPurchase = oi.PriceAtPurchase
                }).ToList();

                return new BaseResponse<ICollection<OrderItemDto>>
                {
                    Message = "Order items retrieved successfully.",
                    Status = true,
                    Data = orderItemDtos
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<OrderItemDto>>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<OrderItemDto>> GetAsync(Guid id)
        {
            try
            {
                var orderItem = await _orderItemRepository.GetOrderByIdAsync(id);
                if (orderItem == null)
                {
                    return new BaseResponse<OrderItemDto>
                    {
                        Message = "Order item not found.",
                        Status = false,
                        Data = null
                    };
                }

                var orderItemDto = new OrderItemDto
                {
                    OrderId = orderItem.OrderId,
                    ProductId = orderItem.ProductId,
                    Quantity = orderItem.Quantity,
                    PriceAtPurchase = orderItem.PriceAtPurchase
                };

                return new BaseResponse<OrderItemDto>
                {
                    Message = "Order item retrieved successfully.",
                    Status = true,
                    Data = orderItemDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderItemDto>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<OrderItemDto>> UpdateOrderItem(UpdateOrderItemRequestModel model)
        {
            try
            {
                var orderItem = await _orderItemRepository.GetOrderByIdAsync(model.Id);
                if (orderItem == null)
                {
                    return new BaseResponse<OrderItemDto>
                    {
                        Message = "Order item not found.",
                        Status = false,
                        Data = null
                    };
                }

                orderItem.Quantity = model.Quantity;
                orderItem.PriceAtPurchase = model.PriceAtPurchase;

                await _orderItemRepository.Update(orderItem);

                var orderItemDto = new OrderItemDto
                {
                    OrderId = orderItem.OrderId,
                    ProductId = orderItem.ProductId,
                    Quantity = orderItem.Quantity,
                    PriceAtPurchase = orderItem.PriceAtPurchase
                };

                return new BaseResponse<OrderItemDto>
                {
                    Message = "Order item updated successfully.",
                    Status = true,
                    Data = orderItemDto
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderItemDto>
                {
                    Message = $"An error occurred: {ex.Message}",
                    Status = false,
                    Data = null
                };
            }
        }
    }
}
