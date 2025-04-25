namespace E_commerce.Core.Entities
{
    public class Category :BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }
}
