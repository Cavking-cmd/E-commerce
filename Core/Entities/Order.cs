namespace E_commerce.Core.Entities
{
    public class Order : BaseEntity
    {
        // Foreignkey for User id
        public DateTime OrderDate  { get; set; }
        public decimal TotalPrice  { get; set; }
        // Possible enum for order status

    }
}
