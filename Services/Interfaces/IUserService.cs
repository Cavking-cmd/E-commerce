using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.UserDtos;

namespace E_commerce.Services.Interfaces
{
    public interface IUserService
    {
        Task<BaseResponse<UserDto>>LoginAsync (LoginRequestModel loginRequest);
        Task<bool> DeleteAsync(Guid id );
        Task<BaseResponse<UserDto>>Update (string email ,LoginRequestModel loginRequest);   
    }
}
