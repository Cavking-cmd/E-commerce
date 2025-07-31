using E_commerce.Core.Entities;

namespace E_commerce.Core.Dtos.Request
{
    public class CartDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid CustomerId { get; set; }
        // Come back and set uo the dto for cart items and coupon dto 
        public ICollection<CartItemDto> CartItems { get; set; } = [];
        public ICollection<CouponDto> AppliedCoupons { get; set; } = [];
    }

    public class CreateCartRequestModel
    {
        public required string Name { get; set; }
    }
}
