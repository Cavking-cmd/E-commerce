using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public SubCategoryService(ISubCategoryRepository subCategoryRepository, ICategoryRepository categoryRepository, IUnitOfWork unitOfWork)
        {
            _subCategoryRepository = subCategoryRepository;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<SubCategoryDto>> CreateSubCategory(CreateSubCategoryRequestModel model)
        {
            try 
            {
                if (Validator.CheckNull(model))
                {
                    return new BaseResponse<SubCategoryDto>
                    {
                        Message = "SubCategory model cannot be null.",
                        Status = false,
                        Data = null,
                    };
                }
                if (Validator.CheckString(model.Name))
                {
                    return new BaseResponse<SubCategoryDto>
                    {
                        Message = "SubCategory name is required.",
                        Status = false,
                        Data = null,
                    };
                }
                var exist = await _subCategoryRepository.CheckAsync(a => a.Name == model.Name);
                if (Validator.CheckDuplicate(exist))
                {
                    return new BaseResponse<SubCategoryDto>
                    {
                        Message = "This SubCategory already exist",
                        Status = false,
                        Data = null 
                    };
                }
                var categoryExistCheck = await _categoryRepository.CheckAsync(a=>a.Id==model.CategoryId);
                if (!categoryExistCheck)
                {
                    return new BaseResponse<SubCategoryDto>
                    {
                        Message="Select a valid category to continue",
                        Status = false,
                        Data = null
                    };
                }
                var subCategory = new SubCategory
                {
                    Name = model.Name,
                    Description = model.Description,
                    CategoryId = model.CategoryId
                };
                await _subCategoryRepository.CreateAsync(subCategory);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<SubCategoryDto>
                {
                    Message ="SubCategory Created Sucessfully",
                    Status = true,
                    Data = new SubCategoryDto
                    {
                        Name = model.Name,
                        Description = model.Description,
                        CategoryId = model.CategoryId
                    }
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<SubCategoryDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<ICollection<SubCategoryDto>>> GetAll()
        {
            try
            {
                var subCategories = await _subCategoryRepository.GetAllSubCategoriesAsync();
                if (subCategories == null)
                {
                    return new BaseResponse<ICollection<SubCategoryDto>>
                    {
                        Message = "SubCategories not found",
                        Status = false,
                        Data = null,
                    };
                }
                var listOfSubCategories = subCategories.Select(a => new SubCategoryDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Description = a.Description,
                    CategoryId = a.CategoryId
                }).ToList();

                return new BaseResponse<ICollection<SubCategoryDto>>
                {
                    Message = "SubCategories found",
                    Status = true,
                    Data = listOfSubCategories
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<SubCategoryDto>>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<SubCategoryDto>> GetAsync(Guid id)
        {
            try
            {
                var subCategory = await _subCategoryRepository.GetSubCategoryByIdAsync(id);
                if (subCategory == null)
                {
                    return new BaseResponse<SubCategoryDto>
                    {
                        Message = "SubCategory not found",
                        Status = false,
                        Data = null,
                    };
                }
                return new BaseResponse<SubCategoryDto>
                {
                    Message = $"SubCategory with {id} found",
                    Status = true,
                    Data = new SubCategoryDto
                    {
                        Id = subCategory.Id,
                        Name = subCategory.Name,
                        Description = subCategory.Description,
                        CategoryId = subCategory.CategoryId
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<SubCategoryDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<SubCategoryDto>> UpdateSubCategory(UpdateSubCategoryRequestModel model)
        {
            try
            {
                if (Validator.CheckNull(model))
                {
                    return new BaseResponse<SubCategoryDto>
                    {
                        Message = "SubCategory model cannot be null.",
                        Status = false,
                        Data = null,
                    };
                }
                if (Validator.CheckString(model.Name))
                {
                    return new BaseResponse<SubCategoryDto>
                    {
                        Message = "SubCategory name is required.",
                        Status = false,
                        Data = null,
                    };
                }
                var subCategory = await _subCategoryRepository.GetSubCategoryByIdAsync(model.Id);
                if (subCategory == null)
                {
                    return new BaseResponse<SubCategoryDto>
                    {
                        Message = "SubCategory not found.",
                        Status = false,
                        Data = null,
                    };
                }
                subCategory.Name = model.Name;
                subCategory.Description = model.Description;
                subCategory.CategoryId = model.CategoryId;
                await _subCategoryRepository.Update(subCategory);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<SubCategoryDto>
                {
                    Message = "SubCategory updated successfully.",
                    Status = true,
                    Data = new SubCategoryDto
                    {
                        Id = subCategory.Id,
                        Name = subCategory.Name,
                        Description = subCategory.Description,
                        CategoryId = subCategory.CategoryId
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<SubCategoryDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteSubCategory(Guid id)
        {
            try
            {
                var subCategory = await _subCategoryRepository.GetSubCategoryByIdAsync(id);
                if (subCategory == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "SubCategory not found",
                        Status = false,
                        Data = false,
                    };
                }
                await _subCategoryRepository.SoftDeleteAsync(subCategory);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<bool>
                {
                    Message = "SubCategory deleted successfully",
                    Status = true,
                    Data = true,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = false,
                };
            }
        }
    }
}

