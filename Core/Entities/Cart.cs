namespace E_commerce.Core.Entities
{
    public class Cart : BaseEntity
    {
        public required string Name { get; set; }
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }

     
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();
    }
}
