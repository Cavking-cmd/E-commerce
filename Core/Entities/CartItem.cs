namespace E_commerce.Core.Entities
{
    public class CartItem : BaseEntity
    {
       // fk for userid and product id 
       public int Quantity { get; set; }

       public Guid CartId { get; set; }
       public Cart? Cart { get; set; }

       public Guid ProductId { get; set; }
       public Product? Product { get; set; }
    }
}
