namespace E_commerce.Core.Dtos.WishlistItemDtos
{
    // DTO for adding an item to a wishlist
    public class CreateWishlistItemRequestModel
    {
        public required Guid WishlistId { get; set; }
        public required Guid ProductId { get; set; }
    }

    // DTO for deleting an item from a wishlist
    public class DeleteWishlistItemRequestModel
    {
        public required Guid WishlistItemId { get; set; }
    }

    // DTO for returning an item in the wishlist
    public class WishlistItemDto
    {
        public required Guid Id { get; set; }
        public required Guid ProductId { get; set; }

        // Optional product snapshot fields for display purposes
        public string? ProductName { get; set; }
        public decimal? ProductPrice { get; set; }
        public string? ProductImageUrl { get; set; }
    }
}
