using E_commerce.Core.Entities;

namespace E_commerce.Core.Dtos.Request
{
    public class OrderItemDto
    {
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public Guid OrderId { get; set; }
        public OrderDto? Order { get; set; }
        public Guid ProductId { get; internal set; }
        public string ProductName { get; internal set; }
    }
    public class CreateOrderItemRequestModel
    {
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
    }

}
