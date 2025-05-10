namespace E_commerce.Core.Dtos.Request
{
    public class VendorDto
    {
        public Guid Id { get; set; }
        public required string BusinessName { get; set; }
        public string? Description { get; set; }
        public required string Email {  get; set; }
        public required string StoreLocation { get; set; }
        // One-to-one relationship with Profile
        public Guid ProfileId { get; set; }
        public UserProfileDto? UserProfile { get; set; }
    }

   
        public class CreateVendorRequestModel
        {
        // Vendor fields
            public required string BusinessName { get; set; }
            public string? Description { get; set; }
            public required string StoreLocation { get; set; }

            // User fields
            public required string Email { get; set; }
            public required string Password { get; set; }

            // UserProfile fields
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



