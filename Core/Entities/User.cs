namespace E_commerce.Core.Entities
{
    public class User : BaseEntity
    {
        public required string Email {  get; set; }
        public required string Password { get; set; }
    }
}
