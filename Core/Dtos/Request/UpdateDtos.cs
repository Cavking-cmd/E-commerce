namespace E_commerce.Core.Dtos.Request
{
    public class UpdateCartRequestModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required Guid CustomerId { get; set; }
    }

    public class UpdateCartItemRequestModel
    {
        public required Guid Id { get; set; }
        public required string ProductName { get; set; }
        public required Guid CartId { get; set; }
        public required Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
    }

    public class UpdateCategoryRequestModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
    }

    public class UpdateCustomerRequestModel
    {
        public required Guid Id { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required Guid ProfileId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string AddressLine { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
    }

    public class UpdateOrderRequestModel
    {
        public required Guid Id { get; set; }
        public required DateTime OrderDate { get; set; }
        public required decimal TotalPrice { get; set; }
        public required int Status { get; set; } // Assuming enum int value
        public required Guid CustomerId { get; set; }
    }

    public class UpdateOrderItemRequestModel
    {
        public required Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public required Guid OrderId { get; set; }
    }

    public class UpdateProductRequestModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public required string ImageUrl { get; set; }
        public required Guid SubCategoryId { get; set; }
    }

    public class UpdateSubCategoryRequestModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required Guid CategoryId { get; set; }
    }

    public class UpdateUserProfileRequestModel
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string Email { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string AddressLine { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
    }

    public class UpdateVendorRequestModel
    {
        public required Guid Id { get; set; }
        public required string BusinessName { get; set; }
        public string? Description { get; set; }
        public required string Email { get; set; }
        public required string StoreLocation { get; set; }
        public required Guid ProfileId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string AddressLine { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
    }
}
