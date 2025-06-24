using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Dtos.WishlistItemDtos;

namespace E_commerce.Core.Dtos.WishlistDtos
{
    // DTO for creating a wishlist
    public class CreateWishlistRequestModel
    {
        public required Guid CustomerId { get; set; }
    }

    // DTO for updating a wishlist
    public class UpdateWishlistRequestModel
    {
        public required Guid Id { get; set; }
        public required Guid CustomerId { get; set; }
    }

    // DTO for deleting a wishlist
    public class WishlistDeleteRequestModel
    {
        public required Guid Id { get; set; }
    }

    // DTO for returning a wishlist
    public class WishlistDto
    {
        public required Guid Id { get; set; }
        public required Guid CustomerId { get; set; }
        public List<WishlistItemDto> WishlistItems { get; set; } = [];
    }
}
