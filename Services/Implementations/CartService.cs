using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork; 
        public CartService(ICartRepository cartRepository, IUnitOfWork unitOfWork)
        {
            _cartRepository = cartRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<CartDto>> CreateCart(CreateCartRequestModel model)
        {
            try
            {
                if (Validator.CheckNull(model))
                {
                    return new BaseResponse<CartDto>
                    {
                        Message = "Cart model cannot be null.",
                        Status = false,
                        Data = null,
                    };
                }
                if (Validator.CheckString(model.Name))
                {
                    return new BaseResponse<CartDto>
                    {
                        Message = "Cart name is required.",
                        Status = false,
                        Data = null,
                    };
                }
                var exist = await _cartRepository.CheckAsync(a => a.Name == model.Name);
                if (Validator.CheckDuplicate(exist))
                {
                    return new BaseResponse<CartDto>()
                    {
                        Message = "Please rename. A cart with this name already exist",
                        Status = false,
                        Data = null,
                    };
                }
                var cart = new Cart
                {
                    Name = model.Name,
                };
                await _cartRepository.CreateAsync(cart);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<CartDto>()
                {
                    Message ="Cart created successfully",
                    Status = true,
                    Data =  new CartDto
                    {
                        Id = cart.Id,
                        Name = cart.Name,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CartDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data =null,
                };
            }
        }
        public async Task<Cart?> GetCartByCustomerIdAsync(Guid customerId)
        {
            return await _cartRepository.GetCartByCustomerIdAsync(customerId);
        }

        public async Task<BaseResponse<ICollection<CartDto>>> GetAll()
        {
            try
            {
                var carts = await _cartRepository.GetAllCartsAsync();
                if (carts == null)
                {
                    return new BaseResponse<ICollection<CartDto>>
                    {
                        Message = "Carts not found",
                        Status = false,
                        Data = null,
                    };
                }
                var listOfCarts = carts.Select(a => new CartDto
                {
                    Id = a.Id,
                    Name = a.Name,
                }).ToList();

                return new BaseResponse<ICollection<CartDto>>
                {
                    Message = "Carts found",
                    Status = true,
                    Data = listOfCarts
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<CartDto>>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<CartDto>> GetAsync(Guid id)
        {
            try
            {
                var cart = await _cartRepository.GetCartByIdAsync(id);
                if (cart == null)
                {
                    return new BaseResponse<CartDto>
                    {
                        Message = "Cart not found",
                        Status = false,
                        Data = null,
                    };
                }
                return new BaseResponse<CartDto>
                {
                    Message = $"Cart with {id} found",
                    Status = true,
                    Data = new CartDto
                    {
                        Id = cart.Id,
                        Name = cart.Name,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CartDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<CartDto>> UpdateCart(UpdateCartRequestModel model)
        {
            try
            {
                if (Validator.CheckNull(model))
                {
                    return new BaseResponse<CartDto>
                    {
                        Message = "Cart model cannot be null.",
                        Status = false,
                        Data = null,
                    };
                }
                if (Validator.CheckString(model.Name))
                {
                    return new BaseResponse<CartDto>
                    {
                        Message = "Cart name is required.",
                        Status = false,
                        Data = null,
                    };
                }
                var cart = await _cartRepository.GetCartByIdAsync(model.Id);
                if (cart == null)
                {
                    return new BaseResponse<CartDto>
                    {
                        Message = "Cart not found.",
                        Status = false,
                        Data = null,
                    };
                }
                cart.Name = model.Name;
                await _cartRepository.Update(cart);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<CartDto>
                {
                    Message = "Cart updated successfully.",
                    Status = true,
                    Data = new CartDto
                    {
                        Id = cart.Id,
                        Name = cart.Name,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CartDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteCart(Guid id)
        {
            try
            {
                var cart = await _cartRepository.GetCartByIdAsync(id);
                if (cart == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "Cart not found",
                        Status = false,
                        Data = false,
                    };
                }
                await _cartRepository.SoftDeleteAsync(cart);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<bool>
                {
                    Message = "Cart deleted successfully",
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

