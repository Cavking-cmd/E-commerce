namespace E_commerce.Core.Entities
{
    public class UserProfile : BaseEntity
    {
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string AddressLine { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string PostalCode { get; set; }
        public required string Country { get; set; }

        // Foreign key to User
        public Guid UserId { get; set; }
        // Navigation property to User
        public User? User { get; set; }
    }
}
