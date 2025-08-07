using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Repositories.Interfaces;
using E_commerce.Core.Dtos.Response; 

namespace E_commerce.Services.Interfaces
{
    public interface IProductService 
    {
        Task<BaseResponse<ProductDto>> CreateProduct(CreateProductRequestModel model);
        Task<BaseResponse<ICollection<ProductDto>>> GetAll();
        Task<BaseResponse<PaginatedResponse<ProductDto>>> GetAllPaginatedAsync(PaginationParams paginationParams);
        Task<BaseResponse<ProductDto>> GetAsync(Guid id);
        Task<BaseResponse<ProductDto>> UpdateProduct(string productName ,UpdateProductModel model);
        Task<BaseResponse<bool>> DeleteProduct(Guid id);

        Task<BaseResponse<ICollection<ProductDto>>> SearchProductsByTagAsync(string tag);
    }
}
