namespace E_commerce.Core.Entities
{
    public class Category : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        //One to Many Relationship with Product
        public ICollection<Product> Products { get; set; }=[];

        // One to Many Relationship with SubCategory
        public ICollection<SubCategory> SubCategories { get; set; } = [];
    }
}
