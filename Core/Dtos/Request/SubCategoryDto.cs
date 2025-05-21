namespace E_commerce.Core.Dtos.Request
{
    public class SubCategoryDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public Guid CategoryId { get; set; }

        public string? CategoryName { get; set; }

        public ICollection<ProductDto> Product { get; set; } = [];
    }
    public class CreateSubCategoryRequestModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }

        public required Guid CategoryId { get; set; } // Reference to the parent category
    }
}
