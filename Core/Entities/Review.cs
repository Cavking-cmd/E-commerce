using System.ComponentModel.DataAnnotations;

namespace E_commerce.Core.Entities
{
    public class Review : BaseEntity
    {
        // fk user id and product id 
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment {  get; set; }
        public DateOnly ReviewDate { get; set; }

    }
}
