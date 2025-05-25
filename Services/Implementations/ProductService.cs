﻿using System.Linq.Expressions;
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

        public ProductService(IProductRepository productRepository, ISubCategoryRepository subCategoryRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _subCategoryRepository = subCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<ProductDto>> CreateProduct(CreateProductRequestModel model)
        {
            try
            {
                if (Validator.CheckNull(model))
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = "Product model cannot be null.",
                        Status = false,
                        Data = null
                    };
                }
                if (Validator.CheckString(model.Name))
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = "Product name is required.",
                        Status = false,
                        Data = null
                    };
                }
                if (Validator.CheckNegativeOrZero(model.Price))
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = "Price cannot be negative or zero.",
                        Status = false,
                        Data = null
                    };
                }
                if (Validator.CheckNegativeOrZero(model.StockQuantity))
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = "Stock quantity cannot be negative or zero.",
                        Status = false,
                        Data = null
                    };
                }

                var subCategoryExists = await _subCategoryRepository.CheckAsync(a => a.Id == model.SubCategoryId);
                if (!subCategoryExists)
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = "The specified subcategory does not exist.",
                        Status = false,
                        Data = null
                    };
                }

                var exist = await _productRepository.CheckAsync(a => a.Name == model.Name && a.SubCategoryId == model.SubCategoryId);
                if (Validator.CheckDuplicate(exist))
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
            var products = await _productRepository.GetAllProductsAsync();
            if (products == null)
            {
                return new BaseResponse<ICollection<ProductDto>>
                {
                    Message = "Products not found",
                    Status = false,
                    Data = null
                };
            }

            var listOfProducts = products.Select(a => new ProductDto
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
                Data = listOfProducts
            };
        }

        public async Task<BaseResponse<ProductDto>> GetAsync(Guid id)
        {
            try
            {
                var product = await _productRepository.GetProductAsync(a => a.Id == id && a.IsDeleted == false);
                if (product == null)
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = $"Product with {id} was not found",
                        Status = false,
                        Data = null
                    };
                }
                return new BaseResponse<ProductDto>
                {
                    Message = $"Product with {id} was found",
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
                    Message = "An error occurred while searching for the product. Please try again.",
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ProductDto>> UpdateProduct(string productName, UpdateProductModel model)
        {
            try
            {
                var product = await _productRepository.GetProductAsync(a => a.Name == productName && !a.IsDeleted);
                if (product == null)
                {
                    return new BaseResponse<ProductDto>
                    {
                        Message = "Product Name not found",
                        Status = false,
                        Data = null
                    };
                }
                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.StockQuantity = model.StockQuantity;
                product.ImageUrl = model.ImageUrl;
                await _productRepository.Update(product);
                return new BaseResponse<ProductDto>
                {
                    Message = "Product has been updated successfully",
                    Status = true,
                    Data = new ProductDto
                    {
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        StockQuantity = product.StockQuantity,
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
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteProduct(Guid id)
        {
            try
            {
                var product = await _productRepository.GetProductAsync(a => a.Id == id && !a.IsDeleted);
                if (product == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "Product not found",
                        Status = false,
                        Data = false
                    };
                }
                await _productRepository.SoftDeleteAsync(product);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<bool>
                {
                    Message = "Product deleted successfully",
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
    }
}

