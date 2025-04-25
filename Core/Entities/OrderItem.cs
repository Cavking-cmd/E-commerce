namespace E_commerce.Core.Entities
{
    public class OrderItem : BaseEntity
    {
        // fk for orderid and product id 
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
    }
}
