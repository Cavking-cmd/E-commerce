﻿using System.Linq.Expressions;
using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Dtos.Response;
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
                    ImageFile = model.ImageFile != null && model.ImageFile.Length > 0 ? new byte[0] : null,
                    ImageMimeType = model.ImageFile != null && model.ImageFile.Length > 0 ? model.ImageFile.ContentType : "image/png"
                };

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await model.ImageFile.CopyToAsync(memoryStream);
                    product.ImageFile = memoryStream.ToArray();
                    product.ImageMimeType = model.ImageFile.ContentType;
                }

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
                        ImageMimeType = product.ImageMimeType,
                        ImageBase64 = product.ImageFile != null ?
                            $"data:{product.ImageMimeType};base64,{Convert.ToBase64String(product.ImageFile)}" : null,
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
                ImageBase64 = a.ImageFile != null ?
                    $"data:{a.ImageMimeType};base64,{Convert.ToBase64String(a.ImageFile)}" : "",
                SubCategoryId = a.SubCategoryId,
            }).ToList();

            return new BaseResponse<ICollection<ProductDto>>
            {
                Message = "Products found",
                Status = true,
                Data = listOfProducts
            };
        }

        public async Task<BaseResponse<PaginatedResponse<ProductDto>>> GetAllPaginatedAsync(PaginationParams paginationParams)
        {
            try
            {
                var allProducts = await _productRepository.GetAllProductsAsync();
                if (allProducts == null)
                {
                    return new BaseResponse<PaginatedResponse<ProductDto>>
                    {
                        Message = "Products not found",
                        Status = false,
                        Data = null
                    };
                }

                var totalCount = allProducts.Count;
                var totalPages = (int)Math.Ceiling(totalCount / (double)paginationParams.PageSize);

                var products = allProducts
                    .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                    .Take(paginationParams.PageSize)
                    .Select(a => new ProductDto
                    {
                        Id = a.Id,
                        Name = a.Name,
                        Description = a.Description,
                        Price = a.Price,
                        StockQuantity = a.StockQuantity,
                        ImageBase64 = a.ImageFile != null ?
                            $"data:{a.ImageMimeType};base64,{Convert.ToBase64String(a.ImageFile)}" : "",
                        SubCategoryId = a.SubCategoryId,
                    })
                    .ToList();

                var paginatedResponse = new PaginatedResponse<ProductDto>
                {
                    PageNumber = paginationParams.PageNumber,
                    PageSize = paginationParams.PageSize,
                    TotalCount = totalCount,
                    TotalPages = totalPages,
                    HasPrevious = paginationParams.PageNumber > 1,
                    HasNext = paginationParams.PageNumber < totalPages,
                    Data = products
                };

                return new BaseResponse<PaginatedResponse<ProductDto>>
                {
                    Message = "Products retrieved successfully",
                    Status = true,
                    Data = paginatedResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<PaginatedResponse<ProductDto>>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null
                };
            }
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
                        ImageMimeType = product.ImageMimeType,
                        ImageBase64 = product.ImageFile != null ?
                            $"data:{product.ImageMimeType};base64,{Convert.ToBase64String(product.ImageFile)}" : null,
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

                if (model.ImageFile != null && model.ImageFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await model.ImageFile.CopyToAsync(memoryStream);
                    product.ImageFile = memoryStream.ToArray();
                    product.ImageMimeType = model.ImageFile.ContentType;
                }

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
                        ImageMimeType = product.ImageMimeType,
                        ImageBase64 = product.ImageFile != null ?
                            $"data:{product.ImageMimeType};base64,{Convert.ToBase64String(product.ImageFile)}" : null,
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

        public async Task<BaseResponse<ICollection<ProductDto>>> SearchProductsByTagAsync(string tag)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tag))
                {
                    return new BaseResponse<ICollection<ProductDto>>
                    {
                        Message = "Search tag cannot be empty.",
                        Status = false,
                        Data = null
                    };
                }

                var allProducts = await _productRepository.GetAllProductsAsync();

                var filteredProducts = allProducts
                    .Where(p => !p.IsDeleted &&
                                !string.IsNullOrEmpty(p.Tags) &&
                                p.Tags.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                      .Any(t => t.Trim().ToLower().Contains(tag.Trim().ToLower())))
                    .Select(p => new ProductDto
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Description = p.Description,
                        Price = p.Price,
                        StockQuantity = p.StockQuantity,
                        ImageMimeType = p.ImageMimeType,
                        ImageBase64 = p.ImageFile != null ?
                            $"data:{p.ImageMimeType};base64,{Convert.ToBase64String(p.ImageFile)}" : null,
                        SubCategoryId = p.SubCategoryId,
                    })
                    .ToList();

                if (!filteredProducts.Any())
                {
                    return new BaseResponse<ICollection<ProductDto>>
                    {
                        Message = $"No products found for tag: '{tag}'",
                        Status = false,
                        Data = null
                    };
                }

                return new BaseResponse<ICollection<ProductDto>>
                {
                    Message = $"{filteredProducts.Count} product(s) found with tag: '{tag}'",
                    Status = true,
                    Data = filteredProducts
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<ProductDto>>
                {
                    Message = $"An error occurred during search: {ex.Message}",
                    Status = false,
                    Data = null
                };
            }
        }
    }
}