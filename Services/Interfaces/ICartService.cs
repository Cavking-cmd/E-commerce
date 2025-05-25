using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Services.Interfaces
{
    public interface ICartService
    {
        Task<BaseResponse<CartDto>> CreateCart(CreateCartRequestModel model);
        Task<BaseResponse<ICollection<CartDto>>> GetAll();
        Task<BaseResponse<CartDto>> GetAsync(Guid id);
        Task<BaseResponse<CartDto>> UpdateCart(UpdateCartRequestModel model);
        Task<BaseResponse<bool>> DeleteCart(Guid id);
    }
}
