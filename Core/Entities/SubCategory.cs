namespace E_commerce.Core.Entities
{
    public class SubCategory : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
