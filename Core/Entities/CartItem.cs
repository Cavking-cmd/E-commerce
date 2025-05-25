namespace E_commerce.Core.Entities
{
    public class CartItem : BaseEntity
    {
       public Guid CartId { get; set; }
       public Cart? Cart { get; set; }
       public Guid ProductId { get; set; }
       public Product? Product { get; set; }

        public int Quantity { get; set; }
       public required string ProductName { get; set; }
       public decimal PricePerUnit { get; set; }
    }
}
