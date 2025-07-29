using E_commerce.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Core.Dtos.Request
{
    public class ReviewDto : BaseEntity
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        //public Guid UserId { get; set; }
        public ProductDto? Product { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateOnly ReviewDate { get; set; }

    }
    public class CreateReviewRequestModel
    {
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateOnly ReviewDate { get; set; }

    }
}
