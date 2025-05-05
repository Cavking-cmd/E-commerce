using System.Diagnostics.Metrics;
using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Implementattions;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_commerce.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IUnitOfWork _unitOfWork;
            public   CustomerService (ICustomerRepository customerRepository, IUserRepository userRepository, IRoleRepository roleRepository , IUserRoleRepository userRoleRepository, IUserProfileRepository profileRepository,IUnitOfWork unitOfWork)
            {
                _customerRepository = customerRepository;
                _userRepository = userRepository;
                _roleRepository = roleRepository;
                _userProfileRepository = profileRepository;
                _userRoleRepository = userRoleRepository;
                _unitOfWork = unitOfWork;
            }

        public async Task<BaseResponse<CustomerDto>> CreateCustomer(CreateCustomerRequestModel model)
        {
            try
            {
                var exist = await _customerRepository.CheckAsync(a => a.Email == model.Email);
                if (exist)
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = "Customer already exist",
                        Status = false,
                        Data = null,
                    };
                }
                var user = new User
                {
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                };
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
                    Country = model.Country
                };
                var customer = new Customer
                {
                    UserId = user.Id,
                    UserName = model.UserName,
                    Email = model.Email,

                };
                var role = await _roleRepository.GetRoleAsync("Customer");
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id,
                    Role = role,
                    User = user
                };
                await _userRepository.CreateAsync(user);
                await _userRoleRepository.CreateAsync(userRole);
                await _customerRepository.CreateAsync(customer);
                await _userProfileRepository.CreateAsync(userProfile);
                await _unitOfWork.SaveChangesAsync();


                return new BaseResponse<CustomerDto>
                {
                    Message = "Acount Created Successfully",
                    Status = true,
                    Data = new CustomerDto
                    {
                        Id = customer.Id,
                        Email = customer.Email,
                        UserName = customer.UserName,
                        UserProfile = new UserProfileDto
                        {
                            Id = userProfile.Id,
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
                return new BaseResponse<CustomerDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null
                };
            }
                
        }
    }
}
