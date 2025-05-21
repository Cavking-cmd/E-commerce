using E_commerce.Core.Entities;

namespace E_commerce.Core.Dtos.Request
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        //One to Many relationship to Review
        public ICollection<ReviewDto> Reviews { get; set; } = [];
        //Many to One relationship with Category
        public Guid CategoryId { get; set; }
        public CategoryDto? Category { get; set; }

        // Many to One relationship with SubCategory
        public Guid? SubCategoryId { get; set; }
        public SubCategoryDto? SubCategory { get; set; }
    }
    public class CreateProductRequestModel
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public required string ImageUrl { get; set; }
       
    }
}
