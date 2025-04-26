using E_commerce.Core.Entities.Enums;

namespace E_commerce.Core.Entities
{
    public class Order : BaseEntity
    {
        // Foreignkey for User id
        public DateTime OrderDate  { get; set; }
        public decimal TotalPrice  { get; set; }
        // Possible enum for order status
        public OrderEnum Status { get; set; }
        // Many to One relationship with Customer
        public Guid CustomerId { get; set; }
        public Customer? Customer { get; set; }
        //One to many relationship with order item 
        public ICollection<OrderItem> OrderItems { get; set; } = [];
        //Many to Many Relationship with Coupon 
        public ICollection<Coupon> Coupons { get; set; } = [];


    }
}
