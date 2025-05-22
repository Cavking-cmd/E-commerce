using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Services.Interfaces
{
    public interface ISubCategoryService
    {
        Task<BaseResponse<SubCategoryDto>> CreateSubCategory(CreateSubCategoryRequestModel model);
    }
}
