namespace E_commerce.Core.Dtos.Request
{
    public class CartItemDto
    {
        public required Guid Id { get; set; }
        public required string ProductName { get; set; }
        public Guid CartId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
    }
    
    public class CreateCartItemRequestModel
    {
        public required string ProductName { get; set; }
        public int Quantity { get; set; }
        public  Guid  ProductId { get; set; }
        public decimal PricePerUnit { get; set; }

    }
}
