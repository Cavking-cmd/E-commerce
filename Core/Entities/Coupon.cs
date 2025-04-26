using E_commerce.Core.Entities.Enums;

namespace E_commerce.Core.Entities
{
    public class Coupon : BaseEntity
    {
        public required string Code { get; set; }
        public decimal  DiscountPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        // enum for status
        public CouponEnum Status { get; set; }
        // Manny to Many Relationship with Order 
        public ICollection<Order> Orders { get; set; } = [];
    }
}
