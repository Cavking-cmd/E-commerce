using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.UserDtos;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
                            UserRoles = user.UserRoles
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

        public async Task<BaseResponse<UserDto>> Update(string email, LoginRequestModel loginRequest)
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
                        UserRoles = user.UserRoles
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
