namespace E_commerce.Core.Entities
{
    public class OrderItem : BaseEntity
    {
        // fk for orderid and product id 
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        //Many to one relationship to Order
        public Guid OrderId { get; set; }
        public Order? Order { get; set; }
        //Many to one relationship to Product   
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        public string ProductName { get;  set; }
    }
}
