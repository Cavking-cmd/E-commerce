using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Services.Interfaces
{
    public interface ISubCategoryService
    {
        Task<BaseResponse<SubCategoryDto>> CreateSubCategory(CreateSubCategoryRequestModel model);
        Task<BaseResponse<ICollection<SubCategoryDto>>> GetAll();
        Task<BaseResponse<SubCategoryDto>> GetAsync(Guid id);
        Task<BaseResponse<SubCategoryDto>> UpdateSubCategory(UpdateSubCategoryRequestModel model);
        Task<BaseResponse<bool>> DeleteSubCategory(Guid id);
    }
}
