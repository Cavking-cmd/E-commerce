  using System.Net.WebSockets;
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
        private readonly IUserService _userService;

        public WishlistService(IWishlistRepository wishlistRepository, IUnitOfWork unitOfWork, IUserService userService)
        {
            _wishlistRepository = wishlistRepository;
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<BaseResponse<WishlistDto>> CreateAsync()
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "User not authenticated",
                    Status = false
                };
            }

            return await CreateWishlistForCustomerAsync(currentUser.Id);
        }

        public async Task<BaseResponse<WishlistDto>> CreateWishlistForCustomerAsync(Guid customerId)
        {
            var existing = await _wishlistRepository.GetWishlistAsync(w => w.CustomerId == customerId);
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
                CustomerId = customerId,
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
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "User not authenticated.",
                    Status = false
                };
            }

            var wishlist = await _wishlistRepository.GetWishlistByIdAsync(model.Id);
            if (wishlist == null || wishlist.CustomerId != currentUser.Id)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "Wishlist not found or access denied.",
                    Status = false
                };
            }

            // Note: Typically you won't update CustomerId. So we skip it here.

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
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new BaseResponse<bool>
                {
                    Message = "User not authenticated.",
                    Status = false,
                    Data = false
                };
            }

            var wishlist = await _wishlistRepository.GetWishlistByIdAsync(id);
            if (wishlist == null || wishlist.CustomerId != currentUser.Id)
            {
                return new BaseResponse<bool>
                {
                    Message = "Wishlist not found or access denied.",
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
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "User not authenticated.",
                    Status = false
                };
            }

            var wishlist = await _wishlistRepository.GetWishlistByIdAsync(id);
            if (wishlist == null || wishlist.CustomerId != currentUser.Id)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "Wishlist not found or access denied.",
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
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new BaseResponse<ICollection<WishlistDto>>
                {
                    Message = "User not authenticated.",
                    Status = false,
                    Data = null
                };
            }

            var wishlists = await _wishlistRepository.GetAllWishlistsAsync();
            var userWishlists = wishlists
                .Where(w => w.CustomerId == currentUser.Id && !w.IsDeleted)
                .Select(w => new WishlistDto
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
                Data = userWishlists
            };
        }

        public async Task<BaseResponse<WishlistDto>> GetWishlistByCustomerIdAsync()
        {
            var currentUser = await _userService.GetCurrentUserAsync();
            if (currentUser == null)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "User not authenticated",
                    Status = false,
                    Data = null
                };
            }

            var wishlist = await _wishlistRepository.GetWishlistAsync(w => w.CustomerId == currentUser.Id && !w.IsDeleted);
            if (wishlist == null)
            {
                return new BaseResponse<WishlistDto>
                {
                    Message = "Wishlist not found",
                    Status = false,
                    Data = null
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
