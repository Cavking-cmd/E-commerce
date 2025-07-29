using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.WishlistDtos;
using E_commerce.Core.Dtos.WishlistItemDtos;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WishlistService(IWishlistRepository wishlistRepository, IUnitOfWork unitOfWork)
        {
            _wishlistRepository = wishlistRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<WishlistDto>> CreateAsync(CreateWishlistRequestModel model)
        {
            if (model.CustomerId == Guid.Empty)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "Customer ID is required.",
                    Status = false
                };
            }

            var existing = await _wishlistRepository.GetWishlistAsync(w => w.CustomerId == model.CustomerId);
            if (existing != null)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "Wishlist already exists for this customer.",
                    Status = false
                };
            }

            var wishlist = new Wishlist
            {
                CustomerId = model.CustomerId,
                WishlistItems = new List<WishlistItem>()
            };

            await _wishlistRepository.CreateAsync(wishlist);
            await _unitOfWork.SaveChangesAsync();

            return new BaseResponse<WishlistDto>
            {
                Message = "Wishlist created successfully.",
                Status = true,
                Data = new WishlistDto
                {
                    Id = wishlist.Id,
                    CustomerId = wishlist.CustomerId,
                    WishlistItems = []
                }
            };
        }

        public async Task<BaseResponse<WishlistDto>> UpdateAsync(UpdateWishlistRequestModel model)
        {
            var wishlist = await _wishlistRepository.GetWishlistByIdAsync(model.Id);
            if (wishlist == null)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "Wishlist not found.",
                    Status = false
                };
            }

            wishlist.CustomerId = model.CustomerId;

            await _wishlistRepository.Update(wishlist);
            await _unitOfWork.SaveChangesAsync();

            return new BaseResponse<WishlistDto>
            {
                Message = "Wishlist updated successfully.",
                Status = true,
                Data = new WishlistDto
                {
                    Id = wishlist.Id,
                    CustomerId = wishlist.CustomerId,
                    WishlistItems = wishlist.WishlistItems.Select(wi => new WishlistItemDto
                    {
                        Id = wi.Id,
                        ProductId = wi.ProductId,
                    }).ToList()
                }
            };
        }

        public async Task<BaseResponse<bool>> DeleteAsync(Guid id)
        {
            var wishlist = await _wishlistRepository.GetWishlistByIdAsync(id);
            if (wishlist == null)
            {
                return new BaseResponse<bool>
                {
                    Message = "Wishlist not found.",
                    Status = false,
                    Data = false
                };
            }

            await _wishlistRepository.SoftDeleteAsync(wishlist);
            await _unitOfWork.SaveChangesAsync();

            return new BaseResponse<bool>
            {
                Message = "Wishlist deleted successfully.",
                Status = true,
                Data = true
            };
        }

        public async Task<BaseResponse<WishlistDto>> GetByIdAsync(Guid id)
        {
            var wishlist = await _wishlistRepository.GetWishlistByIdAsync(id);
            if (wishlist == null)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "Wishlist not found.",
                    Status = false
                };
            }

            return new BaseResponse<WishlistDto>
            {
                Message = "Wishlist found.",
                Status = true,
                Data = new WishlistDto
                {
                    Id = wishlist.Id,
                    CustomerId = wishlist.CustomerId,
                    WishlistItems = wishlist.WishlistItems.Select(wi => new WishlistItemDto
                    {
                        Id = wi.Id,
                        ProductId = wi.ProductId,
                    }).ToList()
                }
            };
        }

        public async Task<BaseResponse<ICollection<WishlistDto>>> GetAllAsync()
        {
            var wishlists = await _wishlistRepository.GetAllWishlistsAsync();

            var result = wishlists.Select(w => new WishlistDto
            {
                Id = w.Id,
                CustomerId = w.CustomerId,
                WishlistItems = w.WishlistItems.Select(wi => new WishlistItemDto
                {
                    Id = wi.Id,
                    ProductId = wi.ProductId,
                }).ToList()
            }).ToList();

            return new BaseResponse<ICollection<WishlistDto>>
            {
                Message = "Wishlists retrieved successfully.",
                Status = true,
                Data = result
            };
        }

        public async Task<BaseResponse<WishlistDto>> GetByCustomerIdAsync(Guid customerId)
        {
            var wishlist = await _wishlistRepository.GetWishlistAsync(w => w.CustomerId == customerId && !w.IsDeleted);
            if (wishlist == null)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "No wishlist found for this customer.",
                    Status = false
                };
            }

            return new BaseResponse<WishlistDto>
            {
                Message = "Wishlist found.",
                Status = true,
                Data = new WishlistDto
                {
                    Id = wishlist.Id,
                    CustomerId = wishlist.CustomerId,
                    WishlistItems = wishlist.WishlistItems.Select(wi => new WishlistItemDto
                    {
                        Id = wi.Id,
                        ProductId = wi.ProductId,
                    }).ToList()
                }
            };
        }
    }
}
