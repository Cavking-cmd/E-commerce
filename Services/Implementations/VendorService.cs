﻿  using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_commerce.Services.Implementations
{
    public class VendorService : IVendorService
    {

        private readonly IVendorRepository _vendorRepository;
        private readonly IUserRepository _userRepository; 
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
        public VendorService (IUserRepository userRepository, IVendorRepository vendorRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IUserProfileRepository userProfileRepository,IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _vendorRepository = vendorRepository;
            _roleRepository = roleRepository;
            _userRoleRepository = userRoleRepository;
            _userProfileRepository = userProfileRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<BaseResponse<VendorDto>> CreateVendor(CreateVendorRequestModel model)
        {
            try
            {
                var exist = await _vendorRepository.CheckAsync(a => a.Email == model.Email);
                if (Validator.CheckDuplicate(exist))
                {
                    return new BaseResponse<VendorDto>()
                    {
                        Message = $"Vendor with {model.Email} already exist",
                        Status = false,
                        Data = null,
                    };
                }
                var user = new User
                {
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password)
                };
                await _userRepository.CreateAsync(user);
                var userProfile = new UserProfile
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    AddressLine = model.AddressLine,
                    City = model.City,
                    State = model.State,
                    PostalCode = model.PostalCode,
                    Country = model.Country,
                    UserId = user.Id
                };
                var vendor = new Vendor
                {
                    BusinessName = model.BusinessName,
                    Email = model.Email,
                    Description = model.Description,
                    StoreLocation = model.StoreLocation
                };
                var role = await _roleRepository.GetRoleAsync("Vendor");
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    Role = role,
                    User = user,
                };

                await _userRoleRepository.CreateAsync(userRole);
                await _vendorRepository.CreateAsync(vendor);
                await _userProfileRepository.CreateAsync(userProfile);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse<VendorDto>
                {
                    Message = "Vendor has been created succesfully",
                    Status = true,
                    Data = new VendorDto
                    {
                        Id = vendor.Id,
                        BusinessName = vendor.BusinessName,
                        Description = vendor.Description,
                        Email = vendor.Email,
                        StoreLocation = vendor.StoreLocation,
                        UserProfile = new UserProfileDto
                        {
                            Id = userProfile.Id,
                            UserId = userProfile.UserId,
                            Email = userProfile.Email,
                            FirstName = userProfile.FirstName,
                            LastName = userProfile.LastName,
                            PhoneNumber = userProfile.PhoneNumber,
                            AddressLine = userProfile.AddressLine,
                            City = userProfile.City,
                            State = userProfile.State,
                            PostalCode = userProfile.PostalCode,
                            Country = userProfile.Country
                        },
                    }
                };
            }
        



            catch (Exception ex)
            {
                return new BaseResponse<VendorDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<VendorDto>> UpdateVendor(UpdateVendorRequestModel model)
        {
            try
            {
                var vendor = await _vendorRepository.GetVendorAsync(a => a.Id == model.Id && !a.IsDeleted);
                if (vendor == null)
                {
                    return new BaseResponse<VendorDto>
                    {
                        Message = "Vendor not found",
                        Status = false,
                        Data = null,
                    };
                }
                vendor.BusinessName = model.BusinessName;
                vendor.Description = model.Description;
                vendor.StoreLocation = model.StoreLocation;
                await _vendorRepository.Update(vendor);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<VendorDto>
                {
                    Message = "Vendor updated successfully",
                    Status = true,
                    Data = new VendorDto
                    {
                        Id = vendor.Id,
                        BusinessName = vendor.BusinessName,
                        Description = vendor.Description,
                        Email = vendor.Email,
                        StoreLocation = vendor.StoreLocation
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<VendorDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteVendor(Guid id)
        {
            try
            {
                var vendor = await _vendorRepository.GetVendorAsync(a => a.Id == id && !a.IsDeleted);
                if (vendor == null)
                {
                    return new BaseResponse<bool>
                    {
                        Message = "Vendor not found",
                        Status = false,
                        Data = false,
                    };
                }
                await _vendorRepository.SoftDeleteAsync(vendor);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<bool>
                {
                    Message = "Vendor deleted successfully",
                    Status = true,
                    Data = true,
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = false,
                };
            }
        }
  

        public async Task<BaseResponse<ICollection<VendorDto>>> GetAll()
        {
            try
            {
                var vendor = await _vendorRepository.GetAllVendorsAsync();
                if (vendor == null)
                    return new BaseResponse<ICollection<VendorDto>>
                    {
                        Message ="Vendors not found",
                        Status = false,
                        Data = null,
                    };
                var listOfVendors = vendor.Select(a => new VendorDto
                {
                    Id = a.Id,
                    BusinessName = a.BusinessName,
                    Description = a.Description,
                    Email = a.Email,
                    StoreLocation = a.StoreLocation,
                    ProfileId = a.User.Profile.Id,
                    UserProfile = new UserProfileDto
                    {
                        UserId = a.User.Profile.UserId,
                        Email = a.User.Profile.Email,
                        FirstName = a.User.Profile.FirstName,
                        LastName = a.User.Profile.LastName,
                        PhoneNumber = a.User.Profile.PhoneNumber,
                        AddressLine = a.User.Profile.AddressLine,
                        City = a.User.Profile.City,
                        State = a.User.Profile.State,
                        PostalCode = a.User.Profile.PostalCode,
                        Country = a.User.Profile.Country
                    }
                }
                );
                return new BaseResponse<ICollection<VendorDto>>
                {
                    Message = "Vendors have been found",
                    Status = true,
                    Data = listOfVendors.ToList()
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<VendorDto>>
                    {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                    };
            }

        }

        public async Task<BaseResponse<VendorDto>> GetAsync(Guid id)
        {
            try
            {
                var vendor = await _vendorRepository.GetVendorAsync(a => a.Id == id && a.IsDeleted == false);
                if (vendor == null)
                {
                    return new BaseResponse<VendorDto>
                    {
                        Message = "Vendor not found",
                        Status = false,
                        Data = null,
                    };
                }
                return new BaseResponse<VendorDto>
                {
                    Message = $"Vendor with {id} has been found ",
                    Status = true,
                    Data = new VendorDto
                    {
                        Id = vendor.Id,
                        Email = vendor.Email,
                        BusinessName = vendor.BusinessName,
                        StoreLocation = vendor.StoreLocation,
                        ProfileId=vendor.User.Profile.Id,
                        UserProfile = new UserProfileDto
                        {
                            UserId = vendor.User.Profile.UserId,
                            Email = vendor.User.Profile.Email,
                            FirstName = vendor.User.Profile.FirstName,
                            LastName = vendor.User.Profile.LastName,
                            PhoneNumber = vendor.User.Profile.PhoneNumber,
                            AddressLine = vendor.User.Profile.AddressLine,
                            City = vendor.User.Profile.City,
                            State = vendor.User.Profile.State,
                            PostalCode = vendor.User.Profile.PostalCode,
                            Country = vendor.User.Profile.Country
                        }

                    }
                };

            } 
            catch (Exception ex)
            {
                return new BaseResponse<VendorDto>
                {
                    Message= ex.Message,
                    Status = false,
                    Data =null,
                };
            }
           
        }
      
    }
}
