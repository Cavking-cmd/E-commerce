using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace E_commerce.Services.Interfaces
{
    public interface IUserProfileService
    {
        Task<BaseResponse<ICollection<UserProfileDto>>> GetAll();
        Task<BaseResponse<UserProfileDto>> GetById(Guid id);
    }
}
