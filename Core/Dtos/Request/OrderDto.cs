using E_commerce.Core.Entities.Enums;
using E_commerce.Core.Entities;

namespace E_commerce.Core.Dtos.Request
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderEnum Status { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerDto? Customer { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; } = [];
        public ICollection<CouponDto> Coupons { get; set; } = [];

    }
    public class CreateOrderRequestModel
    {
        public required string Name { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }


    }

}
