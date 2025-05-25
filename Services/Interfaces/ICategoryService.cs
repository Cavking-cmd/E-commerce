using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<BaseResponse<CategoryDto>> CreateCategory(CreateCategoryRequestModel model);
        Task<BaseResponse<ICollection<CategoryDto>>> GetAll();
        Task<BaseResponse<CategoryDto>> GetAsync(Guid id);
        Task<BaseResponse<CategoryDto>> UpdateCategory(UpdateCategoryRequestModel model);
        Task<BaseResponse<bool>> DeleteCategory(Guid id);
    }
}
