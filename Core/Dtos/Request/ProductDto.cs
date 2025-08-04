using E_commerce.Core.Entities;

namespace E_commerce.Core.Dtos.Request
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        //One to Many relationship to Review
        public ICollection<ReviewDto> Reviews { get; set; } = [];

        // Many to One relationship with SubCategory
        public Guid? SubCategoryId { get; set; }
        public SubCategoryDto? SubCategory { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string? ImageMimeType { get; set; }
        public required string ImageBase64 { get; set; }
    }
    public class CreateProductRequestModel
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public IFormFile?  ImageFile { get; set; }
        public required Guid SubCategoryId { get; set; }

    }
     public class UpdateProductModel
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public IFormFile? ImageFile { get; set; } // Added to handle image updates
        public string? ImageMimeType { get; set; }
        public required string ImageBase64 { get; set; }
    }
}
