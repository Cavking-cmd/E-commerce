namespace E_commerce.Core.Entities
{
    public class Customer : BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public required string Email { get; set; }
        public string? UserName { get; set; }

        //  One-to-one relationship with Cart
        //public Cart? Cart { get; set; }

        //  One-to-one relationship with Wishlist
        public Wishlist? Wishlist { get; set; }

        //  One-to-many relationships
        public ICollection<Cart> Carts { get; set; }
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<Review> Reviews { get; set; } = [];
        public ICollection<ShippingAddress> ShippingAddresses { get; set; } = [];
    }
}
