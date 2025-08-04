﻿namespace E_commerce.Core.Entities
{
    public class Product : BaseEntity
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }  
        public required byte[] ImageFile { get; set; }
        public string? ImageMimeType { get; set; }
        //One to Many relationship to Review
        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        // Many to One relationship with SubCategory
        public Guid? SubCategoryId { get; set; }
        public SubCategory? SubCategory { get; set; }

        // Tags property for search by tag feature
        public string? Tags { get; set; } // e.g., "shoes,running,adidas"
    }
}
