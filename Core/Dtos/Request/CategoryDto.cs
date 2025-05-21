namespace E_commerce.Core.Dtos.Request
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        // Optional: Include related data if needed
        public List<SubCategoryDto>? SubCategories { get; set; }
        public List<ProductDto>? Products { get; set; }
    }

    public class CreateCategoryRequestModel
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
    }

}
