namespace E_commerce.Core.Entities
{
    public class Wishlist : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        public ICollection<WishlistItem> WishlistItems { get; set; } = [];
    }
}
