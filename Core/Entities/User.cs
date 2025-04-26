namespace E_commerce.Core.Entities
{
    public class User : BaseEntity
    {
        public required string Email {  get; set; }
        public required string Password { get; set; }

        // Many to many relationship to roles
        public ICollection<UserRole> UserRoles { get; set; } = [];
        // One to one Relationship to UserProfile
        public UserProfile? Profile { get; set; }
        // One to One relationship with Customer
        public Customer? Customer { get; set; }
        // One to One relationship with Vendor
        public Vendor? Vendor { get; set; }
    }
}
