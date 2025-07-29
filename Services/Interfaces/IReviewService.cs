using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce.Services.Interfaces
{
    public interface IReviewService
    {
        Task<BaseResponse<ReviewDto>> CreateReviewAsync(ReviewDto model);
        Task<BaseResponse<ReviewDto>> GetReviewAsync(Guid id);
        Task<BaseResponse<ICollection<ReviewDto>>> GetAllReviewsAsync();
        Task<BaseResponse<ReviewDto>> UpdateReviewAsync(ReviewDto model);
        Task<BaseResponse<bool>> DeleteReviewAsync(Guid id);
    }
}
