using E_commerce.Core.Entities.Enums;
using E_commerce.Core.Entities;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Core.Dtos
{
    public class CouponDto
    {
        public Guid Id { get; set; }
        public required string Code { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
        // enum for status
        public CouponEnum Status { get; set; }
        // Manny to Many Relationship with Order 
        public ICollection<OrderDto> Orders { get; set; } = [];
    }
    public class CreateCouponRequestModel
    {
        public required string Code { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidUntil { get; set; }
    }
}
