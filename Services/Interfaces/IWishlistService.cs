using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Dtos.WishlistDtos;

namespace E_commerce.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<BaseResponse<WishlistDto>> CreateAsync(CreateWishlistRequestModel model);
        Task<BaseResponse<WishlistDto>> UpdateAsync(UpdateWishlistRequestModel model);
        Task<BaseResponse<bool>> DeleteAsync(Guid id);
        Task<BaseResponse<WishlistDto>> GetByIdAsync(Guid id);
        Task<BaseResponse<ICollection<WishlistDto>>> GetAllAsync();
        Task<BaseResponse<WishlistDto>> GetByCustomerIdAsync(Guid customerId);
    }
}
