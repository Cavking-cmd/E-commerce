namespace E_commerce.Core.Entities
{
    public class ShippingAddress : BaseEntity
    {
        public required string AddressLine { get; set; }
        public required string City {  get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }
    }
}
