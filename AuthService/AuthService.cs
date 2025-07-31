using E_commerce.Core.Dtos.UserDtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;

namespace E_commerce.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;

        public AuthService(IConfiguration config, IUserRepository userRepository)
        {
            _config = config;
            _userRepository = userRepository;
        }

        public async Task<string> AuthenticateAndGenerateTokenAsync(LoginRequestModel loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
                return null;

            var hashedInput = HashPassword(loginDto.Password);
            if (user.Password != hashedInput)
                return null;

            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserRoles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };

            return GenerateToken(userDto);
        }

        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public string GenerateToken(UserDto userDto)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new Claim(ClaimTypes.Email, userDto.Email),
            };

            foreach (var role in userDto.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }


            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwt["ExpiryTime"])),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
