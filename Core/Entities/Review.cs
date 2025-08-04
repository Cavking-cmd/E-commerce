using System.ComponentModel.DataAnnotations;

namespace E_commerce.Core.Entities
{
    public class Review : BaseEntity
    {
       
        public Guid ProductId { get; set; }
        public Product? Product { get; set; }
        [Range(1, 5)]
        public int Rating { get; set; }
        public string? Comment {  get; set; }
        public DateTime ReviewDate { get; set; }
        // If possible implement the user relationship 


    }

}
