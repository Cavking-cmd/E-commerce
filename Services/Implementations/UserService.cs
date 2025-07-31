using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.UserDtos;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var user = await _userRepository.GetAsync(id);
                if (user == null) return false;

                await _userRepository.SoftDeleteAsync(user);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<UserDto> GetCurrentUserAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null || httpContext.User == null)
            {
                return null;
            }

            var emailClaim = httpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            if (emailClaim == null)
            {
                return null;
            }

            var user = await _userRepository.GetUserAsync(u => u.Email == emailClaim.Value);
            if (user == null)
            {
                return null;
            }

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                UserRoles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
            };
        }

        public async Task<BaseResponse<UserDto>> LoginAsync(LoginRequestModel loginRequest)
        {
            try
            {
                if (Validator.CheckNull(loginRequest))
                {
                    return new BaseResponse<UserDto>
                    {
                        Message = "Login request cannot be null.",
                        Status = false,
                        Data = null
                    };
                }
                if (Validator.CheckString(loginRequest.Email))
                {
                    return new BaseResponse<UserDto>
                    {
                        Message = "Email is required.",
                        Status = false,
                        Data = null
                    };
                }
                if (Validator.CheckString(loginRequest.Password))
                {
                    return new BaseResponse<UserDto>
                    {
                        Message = "Password is required.",
                        Status = false,
                        Data = null
                    };
                }

                var user = await _userRepository.GetUserAsync(a => a.Email == loginRequest.Email);
                if (user == null)
                {
                    return new BaseResponse<UserDto>
                    {
                        Message = "The email does not exist.",
                        Status = false,
                        Data = null
                    };
                }

                if (BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
                {
                    return new BaseResponse<UserDto>
                    {
                        Message = "Login successful.",
                        Status = true,
                        Data = new UserDto
                        {
                            Id = user.Id,
                            Email = user.Email,
                            UserRoles = user.UserRoles.Select(ur=>ur.Role.Name).ToList()
                        }
                    };
                }

                return new BaseResponse<UserDto>
                {
                    Message = "Invalid credentials.",
                    Status = false,
                    Data = null
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BaseResponse<UserDto>
                {
                    Message = "An error occurred while logging in.",
                    Status = false,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<UserDto>> Update(string email, UpdateloginRequest loginRequest)
        {
            try
            {
                if (Validator.CheckString(email))
                {
                    return new BaseResponse<UserDto>
                    {
                        Message = "Email is required.",
                        Status = false,
                        Data = null
                    };
                }
                if (Validator.CheckNull(loginRequest))
                {
                    return new BaseResponse<UserDto>
                    {
                        Message = "Update request cannot be null.",
                        Status = false,
                        Data = null
                    };
                }
                if (Validator.CheckString(loginRequest.Password))
                {
                    return new BaseResponse<UserDto>
                    {
                        Message = "Password is required.",
                        Status = false,
                        Data = null
                    };
                }

                var user = await _userRepository.GetUserAsync(a => a.Email == email);
                if (user == null)
                {
                    return new BaseResponse<UserDto>
                    {
                        Message = "User not found.",
                        Status = false,
                        Data = null
                    };
                }

                user.Password = BCrypt.Net.BCrypt.HashPassword(loginRequest.Password);
                await _userRepository.Update(user);

                return new BaseResponse<UserDto>
                {
                    Message = "Password has been changed successfully.",
                    Status = true,
                    Data = new UserDto
                    {
                        Id = user.Id,
                        Email = user.Email,
                        UserRoles = user.UserRoles.Select(ur => ur.Role.Name).ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new BaseResponse<UserDto>
                {
                    Message = "An error occurred while updating the password.",
                    Status = false,
                    Data = null
                };
            }
        }
    }
}
