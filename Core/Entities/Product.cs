namespace E_commerce.Core.Entities
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public required string ImageUrl {  get; set; }
        //One to Many relationship to Review
        public ICollection<Review> Reviews { get; set; } 
        //Many to One relationship with Category
        public Guid CategoryId { get; set; }
        public Category? Category { get; set; }

        // Many to One relationship with SubCategory
        public Guid? SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }
    }
}
