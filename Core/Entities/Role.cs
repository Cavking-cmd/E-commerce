namespace E_commerce.Core.Entities
{
    public class Role : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        // Many to many relationship to User
        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
