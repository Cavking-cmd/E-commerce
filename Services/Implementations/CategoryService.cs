using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async  Task<BaseResponse<CategoryDto>> CreateCategory(CreateCategoryRequestModel model)
        {
            try
            {
                var exist = await _categoryRepository.CheckAsync(a => a.Name == model.Name);
                if (exist)
                {
                    return new BaseResponse<CategoryDto>
                    {
                        Message = "This category already exist",
                        Status = false,
                        Data = null
                    };
                }
                var category = new Category
                {
                    Name = model.Name,
                    Description = model.Description,
                };
                 await _categoryRepository.CreateAsync(category);
                return new BaseResponse<CategoryDto>
                {
                    Message = "Category created Sucessfully",
                    Status = true,
                    Data = new CategoryDto
                    {
                        Name = model.Name,
                        Description = model.Description,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CategoryDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null
                };
            }
        }
    }
}
