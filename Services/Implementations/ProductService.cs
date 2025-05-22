using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        public async Task<BaseResponse<ProductDto>> CreateProduct(CreateProductRequestModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = "Product name is required.",
                        Status = false,
                        Data = null
                    };
                }
                if (model.Price < 0)
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = "Price cannot be negative.",
                        Status = false,
                        Data = null
                    };
                }
                if (model.StockQuantity < 0)
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = "Stock quantity cannot be negative.",
                        Status = false,
                        Data = null
                    };
                }

                var subCategory = await _subCategoryRepository.CheckAsync(a => a.Id == model.SubCategoryId);
                if (!subCategory)
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = "The specified subcategory does not exist.",
                        Status = false,
                        Data = null
                    };
                }

                var exist = await _productRepository.CheckAsync(a => a.Name == model.Name && a.SubCategoryId == model.SubCategoryId);
                if (exist)
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = "This product already exists in the specified subcategory.",
                        Status = false,
                        Data = null
                    };
                }

                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    StockQuantity = model.StockQuantity,
                    ImageUrl = model.ImageUrl,
                    SubCategoryId = model.SubCategoryId
                };

                await _productRepository.CreateAsync(product);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse<ProductDto>
                {
                    Message = "Product created successfully",
                    Status = true,
                    Data = new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        StockQuantity = product.StockQuantity,
                        ImageUrl = product.ImageUrl,
                        SubCategoryId = product.SubCategoryId,
                    }
                };
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework)
                return new BaseResponse<ProductDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ICollection<ProductDto>>> GetAll()
        {
            var product = await _productRepository.GetAllProducts();
            if (product == null)
            {
                return new BaseResponse<ICollection<ProductDto>>
                {
                    Message = "Product not found",
                    Status = false,
                    Data = null
                };
            }

            var listOfProduct = product.Select(a => new ProductDto
            {
                Id = a.Id,
                Name = a.Name,
                Description = a.Description,
                Price = a.Price,
                StockQuantity = a.StockQuantity,
                ImageUrl = a.ImageUrl,
                SubCategoryId = a.SubCategoryId,
            }).ToList();

            return new BaseResponse<ICollection<ProductDto>>
            {
                Message = "Products found",
                Status = true,
                Data = listOfProduct
            };
        }

        public async Task<BaseResponse<ProductDto>> GetAsync(Guid id)
        {
            try
            {
                var product = await _productRepository.GetProduct(a => a.Id == id && a.IsDeleted == false);
                if (product == null)
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = $"Product with {id} was not found ",
                        Status = false,
                        Data = null
                    };
                }
                return new BaseResponse<ProductDto>
                {
                    Message = $"Product with {id} was not found ",
                    Status = true,
                    Data = new ProductDto
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        StockQuantity = product.StockQuantity,
                        ImageUrl = product.ImageUrl,
                        SubCategoryId = product.SubCategoryId,
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ProductDto>
                {
                    Message = "An error occurred while search for  the product. Please try again.",
                    Status = false,
                    Data = null
                };
            }
    }

        public async Task<BaseResponse<ProductDto>> UpdateProduct(string productName, UpdateProductModel model)
        {
            try 
            {
                var product = await _productRepository.GetProduct(a=>a.Name == productName && !a.IsDeleted);
                if(product == null)
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message="Product Name not found",
                        Status = false,
                        Data = null
                    };
                }
                product.Name =(model.Name);
                product.Description =(model.Description);
                product.Price =(model.Price);
                product.StockQuantity =(model.StockQuantity);
                product.ImageUrl =(model.ImageUrl);
                await _productRepository.Update(product);
                return new BaseResponse<ProductDto>
                {
                    Message="Product has been updated successfully",
                    Status = true,
                    Data= new ProductDto
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price=product.Price,
                        StockQuantity=product.StockQuantity,
                        ImageUrl = product.ImageUrl,
                    }

                };

            }
            catch (Exception ex) 
            {
                return new BaseResponse<ProductDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data=null,
                };
            }
        }
    }
}
