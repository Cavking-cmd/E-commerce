using E_commerce.Core.Dtos.UserDtos;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_commerce.Core.Entities;
using Microsoft.AspNetCore.Identity;
using E_commerce.Repositories.Interfaces;

namespace E_commerce.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository; // Assume this handles DB access
        private readonly IPasswordHasher<User> _passwordHasher;

        public AuthService(IConfiguration config, IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
        {
            _config = config;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        // ✅ This method handles both authentication and token generation
        public async Task<string> AuthenticateAndGenerateTokenAsync(LoginRequestModel loginDto)
        {
            // Step 1: Find user by email
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null)
                return null;

            // Step 2: Verify password
            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password);
            if (result != PasswordVerificationResult.Success)
                return null;

            // Step 3: Generate token if authentication passes
            var userDto = new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserRoles = user.UserRoles
            };

            return GenerateToken(userDto);
        }

        // 🔐 This part generates the JWT (no changes here)
        public string GenerateToken(UserDto userDto)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userDto.Id.ToString()),
                new Claim(ClaimTypes.Email, userDto.Email),
            };

            foreach (var item in userDto.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item.Role.Name));
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
