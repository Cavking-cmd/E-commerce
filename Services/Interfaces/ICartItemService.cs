using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Services.Interfaces
{
    public interface ICartItemService
    {
        Task<BaseResponse<CartItemDto>> CreateCartItem(CreateCartItemRequestModel model);
        Task<BaseResponse<ICollection<CartItemDto>>> GetAll();
        Task<BaseResponse<CartItemDto>> GetAsync(Guid id);
        Task<BaseResponse<CartItemDto>> UpdateCartItem(UpdateCartItemRequestModel model);
        Task<BaseResponse<bool>> DeleteCartItem(Guid id);
    }
}
