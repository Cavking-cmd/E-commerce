using E_commerce.Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace E_commerce.Core.Dtos.UserDtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public required string Email { get; set; }
        public  List<string>  UserRoles { get; set; } = [];
    }

    public class LoginRequestModel
    {
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        public required string Password { get; set; }
    }
    public class UpdateloginRequest
    {
        [Required]
        public required string Password { get; set; }
    }

}
