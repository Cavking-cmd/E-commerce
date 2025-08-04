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
        private readonly IUnitOfWork _unitOfWork;
        public CategoryService(ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<CategoryDto>> CreateCategory(CreateCategoryRequestModel model)
        {
            
            try
            {
                if (Validator.CheckNull(model))
                {
                    return new BaseResponse<CategoryDto>
                    {
                        Message = "Category model cannot be null.",
                        Status = false,
                        Data = null,
                    };
                }
                if (Validator.CheckString(model.Name))
                {
                    return new BaseResponse<CategoryDto>
                    {
                        Message = "Category name is required.",
                        Status = false,
                        Data = null,
                    };
                }
                var exist = await _categoryRepository.CheckAsync(a => a.Name == model.Name);
                if (Validator.CheckDuplicate(exist))
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
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<CategoryDto>
                {
                    Message = "Category created Sucessfully",
                    Status = true,
                    Data = new CategoryDto
                    {
                        Id= category.Id,
                        Name = model.Name,
                        Description = model.Description
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
                
        public async Task<BaseResponse<ICollection<CategoryDto>>> GetAll()
        {
            var categories = await _categoryRepository.GetAllCategoriesAsync();
            if (categories == null)
            {
                return new BaseResponse<ICollection<CategoryDto>>
                {
                    Message = "Categories not found",
                    Status = false,
                    Data = null
                };
            }

            var listOfCategories = categories.Select(a => new CategoryDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
            }).ToList();

            return new BaseResponse<ICollection<CategoryDto>>
            {
                Message = "Categories found",
                Status = true,
                Data = listOfCategories
            };
        }

        public async Task<BaseResponse<CategoryDto>> GetAsync(Guid id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return new BaseResponse<CategoryDto>
                    {
                        Message = $"Category with {id} was not found",
                        Status = false,
                        Data = null
                    };
                }
                return new BaseResponse<CategoryDto>
                {
                    Message = $"Category with {id} was found",
                    Status = true,
                    Data = new CategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CategoryDto>
                {
                    Message = "An error occurred while searching for the category. Please try again.",
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteCategory(Guid id)
        {
            try
            {
                var category = await _categoryRepository.GetCategoryByIdAsync(id);
                if (category == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "Category not found",
                        Status = false,
                        Data = false
                    };
                }
                await _categoryRepository.SoftDeleteAsync(category);
                return new BaseResponse<bool>
                {
                    Message = "Category deleted successfully",
                    Status = true,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = false
                };
            }
        }

  
        public async Task<BaseResponse<CategoryDto>> UpdateCategory(UpdateCategoryRequestModel model)
        {
            try
            {
                if (Validator.CheckNull(model))
                {
                    return new BaseResponse<CategoryDto>
                    {
                        Message = "Category model cannot be null.",
                        Status = false,
                        Data = null,
                    };
                }
                if (Validator.CheckString(model.Name))
                {
                    return new BaseResponse<CategoryDto>
                    {
                        Message = "Category name is required.",
                        Status = false,
                        Data = null,
                    };
                }
                var category = await _categoryRepository.GetCategoryByIdAsync(model.Id);
                if (category == null)
                {
                    return new BaseResponse<CategoryDto>
                    {
                        Message = "Category not found.",
                        Status = false,
                        Data = null,
                    };
                }
                category.Name = model.Name;
                category.Description = model.Description;
                await _categoryRepository.Update(category);
                return new BaseResponse<CategoryDto>
                {
                    Message = "Category updated successfully.",
                    Status = true,
                    Data = new CategoryDto
                    {
                        Id = category.Id,
                        Name = category.Name,
                        Description = category.Description,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CategoryDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }
    }
}
