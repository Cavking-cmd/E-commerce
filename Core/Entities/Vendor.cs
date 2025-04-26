namespace E_commerce.Core.Entities
{
    public class Vendor : BaseEntity
    {
        public required string BusinessName { get; set; }
        public string? Description { get; set; }
        public required string BusinessEmail { get; set; }
        public required string StoreLocation { get; set; }
        // One to one relationship with User (Chain : Vendor to user to profile)
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
