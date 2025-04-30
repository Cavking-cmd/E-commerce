using E_commerce.Core.Dtos.UserDtos;

namespace E_commerce.AuthService
{
    public interface IAuthService
    {
        string GenerateToken(UserDto userDto);
    }
}
