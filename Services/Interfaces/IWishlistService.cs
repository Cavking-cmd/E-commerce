using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Dtos.WishlistDtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce.Services.Interfaces
{
    public interface IWishlistService
    {
        Task<BaseResponse<WishlistDto>> CreateWishlist(CreateWishlistRequestModel model);
        Task<BaseResponse<ICollection<WishlistDto>>> GetAll();
        Task<BaseResponse<WishlistDto>> GetAsync(Guid id);
        Task<BaseResponse<WishlistDto>> UpdateWishlist(UpdateWishlistRequestModel model);
        Task<BaseResponse<bool>> DeleteWishlist(Guid id);
    }
}
