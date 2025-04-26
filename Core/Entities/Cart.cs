namespace E_commerce.Core.Entities
{
    public class Cart : BaseEntity
    {
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

        // Optional: If vendors can have carts, add Vendor relationship
        public Guid? VendorId { get; set; }
        public Vendor? Vendor { get; set; }

        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
    }
}
