using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Repositories.Interfaces;

namespace E_commerce.Services.Interfaces
{
    public interface IProductService 
    {
        Task<BaseResponse<ProductDto>> CreateProduct(CreateProductRequestModel model);
        Task<BaseResponse<ICollection<ProductDto>>> GetAll();
        Task<BaseResponse<ProductDto>> GetAsync(Guid id);
        Task<BaseResponse<ProductDto>> UpdateProduct(string productName ,UpdateProductModel model);
        Task<BaseResponse<bool>> DeleteProduct(Guid id);
    }
}
