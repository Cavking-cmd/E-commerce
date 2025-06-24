using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IUnitOfWork _unitOfWork;
        public CartItemService(ICartItemRepository  cartItemRepository, IUnitOfWork unitOfWork)
        {
            _cartItemRepository = cartItemRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CartItemDto>> CreateCartItem(CreateCartItemRequestModel model)
        {
            try
            {
                if (Validator.CheckNull(model))
                {
                    return new BaseResponse<CartItemDto>
                    {
                        Message = "CartItem model cannot be null.",
                        Status = false,
                        Data = null,
                    };
                }
               
                var exist = await _cartItemRepository.CheckAsync(a => a.ProductId == model.ProductId);
                if (Validator.CheckDuplicate(exist))
                {
                    return new BaseResponse<CartItemDto>()
                    {
                        Message = "CartItem with this productId already exists.",
                        Status = false,
                        Data = null,
                    };
                }
                var cartItem = new CartItem
                {
                    ProductName=model.ProductName,
                    ProductId = model.ProductId,
                    Quantity = model.Quantity,
                    PricePerUnit = model.PricePerUnit,
                };
                await _cartItemRepository.CreateAsync(cartItem);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<CartItemDto>
                {
                    Message = "CartItem created succesfully",
                    Status = true,
                    Data =  new CartItemDto
                    {
                        Id= cartItem.Id,
                        ProductName = model.ProductName,
                        ProductId = model.ProductId,
                        Quantity = model.Quantity,
                        PricePerUnit = model.PricePerUnit,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CartItemDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }
        

        public async Task<BaseResponse<ICollection<CartItemDto>>> GetAll()
        {
            try
            {
                var cartItems = await _cartItemRepository.GetAllCartItemsAsync();
                if (cartItems == null)
                {
                    return new BaseResponse<ICollection<CartItemDto>>
                    {
                        Message = "CartItems not found",
                        Status = false,
                        Data = null,
                    };
                }
                var listOfCartItems = cartItems.Select(a => new CartItemDto
                {
                    Id = a.Id,
                    ProductName = a.ProductName,
                    ProductId = a.ProductId,
                    Quantity = a.Quantity,
                    PricePerUnit = a.PricePerUnit,
                }).ToList();

                return new BaseResponse<ICollection<CartItemDto>>
                {
                    Message = "CartItems found",
                    Status = true,
                    Data = listOfCartItems
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<CartItemDto>>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<CartItemDto>> GetAsync(Guid id)
        {
            try
            {
                var cartItem = await _cartItemRepository.GetCartItemByIdAsync(id);
                if (cartItem == null)
                {
                    return new BaseResponse<CartItemDto>
                    {
                        Message = "CartItem not found",
                        Status = false,
                        Data = null,
                    };
                }
                return new BaseResponse<CartItemDto>
                {
                    Message = $"CartItem with {id} found",
                    Status = true,
                    Data = new CartItemDto
                    {
                        Id = cartItem.Id,
                        ProductName = cartItem.ProductName,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        PricePerUnit = cartItem.PricePerUnit,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CartItemDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<CartItemDto>> UpdateCartItem(UpdateCartItemRequestModel model)
        {
            try
            {
                if (Validator.CheckNull(model))
                {
                    return new BaseResponse<CartItemDto>
                    {
                        Message = "CartItem model cannot be null.",
                        Status = false,
                        Data = null,
                    };
                }
                var cartItem = await _cartItemRepository.GetCartItemByIdAsync(model.Id);
                if (cartItem == null)
                {
                    return new BaseResponse<CartItemDto>
                    {
                        Message = "CartItem not found.",
                        Status = false,
                        Data = null,
                    };
                }
                cartItem.ProductName = model.ProductName;
                cartItem.ProductId = model.ProductId;
                cartItem.Quantity = model.Quantity;
                cartItem.PricePerUnit = model.PricePerUnit;
                await _cartItemRepository.Update(cartItem);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<CartItemDto>
                {
                    Message = "CartItem updated successfully.",
                    Status = true,
                    Data = new CartItemDto
                    {
                        Id = cartItem.Id,
                        ProductName = cartItem.ProductName,
                        ProductId = cartItem.ProductId,
                        Quantity = cartItem.Quantity,
                        PricePerUnit = cartItem.PricePerUnit,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CartItemDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteCartItem(Guid id)
        {
            try
            {
                var cartItem = await _cartItemRepository.GetCartItemByIdAsync(id);
                if (cartItem == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "CartItem not found",
                        Status = false,
                        Data = false,
                    };
                }
                await _cartItemRepository.SoftDeleteAsync(cartItem);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<bool>
                {
                    Message = "CartItem deleted successfully",
                    Status = true,
                    Data = true,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = false,
                };
            }
        }
    }
}

