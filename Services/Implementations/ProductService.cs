using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }
        public async Task<BaseResponse<ProductDto>> CreateProduct(CreateProductRequestModel model)
        {
            try
            {
                var exist = await _repository.CheckAsync(a => a.Name == model.Name);
                if (exist)
                    return new BaseResponse<ProductDto>
                    {
                        Message = "This product already exist",
                        Status = false,
                        Data = null
                    };
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    StockQuantity = model.StockQuantity,
                    ImageUrl = model.ImageUrl,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProductDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null
                };
            }
        }

        public Task<BaseResponse<ICollection<ProductDto>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<ProductDto>> GetAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
