using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_commerce.Services.Implementations
{
    public class UserProfileService : IUserProfileService
    {
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserProfileService(IUserProfileRepository userProfileRepository, IUnitOfWork unitOfWork)
        {
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<ICollection<UserProfileDto>>> GetAll()
        {
            var profiles = await _userProfileRepository.GetAll();
            if (profiles == null || !profiles.Any())
            {
                return new BaseResponse<ICollection<UserProfileDto>>
                {
                    Message = "User profiles not found",
                    Status = false,
                    Data = null
                };
            }

            var profileDtos = profiles.Select(p => new UserProfileDto
            {
                Id = p.Id,
                UserId = p.UserId,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Email = p.Email,
                PhoneNumber = p.PhoneNumber,
                AddressLine = p.AddressLine,
                City = p.City,
                State = p.State,
                PostalCode = p.PostalCode,
                Country = p.Country
            }).ToList();

            return new BaseResponse<ICollection<UserProfileDto>>
            {
                Message = "User profiles found",
                Status = true,
                Data = profileDtos
            };
        }

        public async Task<BaseResponse<UserProfileDto>> GetById(Guid id)
        {
            var profile = await _userProfileRepository.GetProfileByIdAsync(id);
            if (profile == null)
            {
                return new BaseResponse<UserProfileDto>
                {
                    Message = $"User profile with id {id} not found",
                    Status = false,
                    Data = null
                };
            }

            var profileDto = new UserProfileDto
            {
                Id = profile.Id,
                UserId = profile.UserId,
                FirstName = profile.FirstName,
                LastName = profile.LastName,
                Email = profile.Email,
                PhoneNumber = profile.PhoneNumber,
                AddressLine = profile.AddressLine,
                City = profile.City,
                State = profile.State,
                PostalCode = profile.PostalCode,
                Country = profile.Country
            };

            return new BaseResponse<UserProfileDto>
            {
                Message = $"User profile with id {id} found",
                Status = true,
                Data = profileDto
            };
        }
    }
}
