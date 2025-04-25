namespace E_commerce.Core.Entities
{
    public class Coupon : BaseEntity
    {
        public required string Code { get; set; }
        public decimal  DiscountPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        // enum for status 
    }
}
