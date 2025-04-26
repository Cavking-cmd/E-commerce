namespace E_commerce.Core.Entities
{
    public class CouponOrder : BaseEntity
    {
        public Guid CouponId { get; set; }
        public Coupon? Coupon { get; set; }
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
    }
}
