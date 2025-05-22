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
        public SubCategoryService(ISubCategoryRepository subCategoryRepository, IUnitOfWork unitOfWork)
        {
            _subCategoryRepository = subCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<SubCategoryDto>> CreateSubCategory(CreateSubCategoryRequestModel model)
        {
            try 
            {
                var exist = await _subCategoryRepository.CheckAsync(a => a.Name == model.Name);
                if (exist)
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
    }
}
