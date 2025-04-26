namespace E_commerce.Core.Entities
{
    public class Customer : BaseEntity
    {
        //public Guid UserId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        // One to many relationship withCartItem
        public ICollection<CartItem> CartItems { get; set; } = [];
        //One to many relationship with wishlistitems
        public ICollection<WishlistItem> WishlistItems { get; set; } = [];
        //One to Many Relationship with Order
        public ICollection<Order>Orders { get; set; } = [];
        //One to many relationship with review
        public ICollection<Review>Reviews { get; set; } = [];
        // One to Many relationship with shipping address
        public ICollection<ShippingAddress> ShippingAddresses { get; set; }

    }
}
