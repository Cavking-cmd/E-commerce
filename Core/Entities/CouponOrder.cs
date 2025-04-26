namespace E_commerce.Core.Entities
{
    public class CouponOrder
    {
        public Guid CouponId { get; set; }
        public Coupon? Coupon { get; set; }
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
