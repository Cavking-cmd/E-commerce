namespace E_commerce.Core.Entities
{
    public class CartItem : BaseEntity
    {
       // fk for userid and product id 
       public int Quantity { get; set; }
    }
}
