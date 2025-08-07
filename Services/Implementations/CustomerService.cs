using System.Diagnostics.Metrics;
using System.Security.Cryptography.Xml;
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
        private readonly IWishlistService _wishlistService;
        private readonly ICartService _cartService;
        public CustomerService(ICustomerRepository customerRepository, IUserRepository userRepository, IRoleRepository roleRepository, IUserRoleRepository userRoleRepository, IUserProfileRepository profileRepository, IUnitOfWork unitOfWork, IWishlistService wishlistService, ICartService cartService)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userProfileRepository = profileRepository;
            _userRoleRepository = userRoleRepository;
            _unitOfWork = unitOfWork;
            _wishlistService = wishlistService;
            _cartService = cartService;
        }

        public async Task<BaseResponse<CustomerDto>> CreateCustomer(CreateCustomerRequestModel model)
        {
            try
            {
                if (Validator.CheckNull(model))
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = "Customer model cannot be null.",
                        Status = false,
                        Data = null,
                    };
                }
                if (Validator.CheckString(model.Email))
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = "Email is required.",
                        Status = false,
                        Data = null,
                    };
                }
                if (Validator.CheckString(model.Password))
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = "Password is required.",
                        Status = false,
                        Data = null,
                    };
                }
                if (Validator.CheckString(model.UserName))
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = "UserName is required.",
                        Status = false,
                        Data = null,
                    };
                }
                var exist = await _customerRepository.CheckAsync(a => a.Email == model.Email);
                if (Validator.CheckDuplicate(exist))
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = $"Customer with {model.Email} already exist",
                        Status = false,
                        Data = null,
                    };
                }
                var user = new User
                {
                    Email = model.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
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
                await _userRoleRepository.CreateAsync(userRole);
                await _customerRepository.CreateAsync(customer);
                await _userProfileRepository.CreateAsync(userProfile);
                await _unitOfWork.SaveChangesAsync();

                // Create a wishlist for the new customer
                try
                {
                    var wishlistResponse = await _wishlistService.CreateWishlistForCustomerAsync(customer.Id);
                    if (!wishlistResponse.Status)
                    {
                        return new BaseResponse<CustomerDto>
                        {
                            Message = $"Failed to create wishlist for customer {customer.Id}: {wishlistResponse.Message}",
                            Status = false,
                            Data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = $"Exception occurred while creating wishlist for customer {customer.Id}: {ex.Message}",
                        Status = false,
                        Data = null
                    };
                  }

                // Create a cart for the new customer
                try
                {
                    var cartResponse = await _cartService.CreateCart(new CreateCartRequestModel { Name = $"Cart1" });
                    if (!cartResponse.Status)
                    {
                        return new BaseResponse<CustomerDto>
                        {
                            Message = $"Failed to create cart for customer {customer.Id}: {cartResponse.Message}",
                            Status = false,
                            Data = null
                        };
                    }
                }
                catch (Exception ex)
                {
                    // Log the exception but don't fail the customer creation
                    Console.WriteLine($"Exception occurred while creating cart for customer {customer.Id}: {ex.Message}");
                }

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
                return new BaseResponse<CustomerDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null
                };
            }
        }
        

        public async Task<BaseResponse<CustomerDto>> UpdateCustomer(UpdateCustomerRequestModel model)
        {
            try
            {
                if (Validator.CheckNull(model))
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = "Customer model cannot be null.",
                        Status = false,
                        Data = null,
                    };
                }
                if (Validator.CheckString(model.Email))
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = "Email is required.",
                        Status = false,
                        Data = null,
                    };
                }
                if (Validator.CheckString(model.UserName))
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = "UserName is required.",
                        Status = false,
                        Data = null,
                    };
                }
                var customer = await _customerRepository.GetAsync(a => a.Id == model.Id);
                if (customer == null)
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = "Customer not found.",
                        Status = false,
                        Data = null,
                    };
                }
                var userProfile = await _userProfileRepository.GetProfileAsync(a => a.Id == model.ProfileId);
                if (userProfile == null)
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = "User profile not found.",
                        Status = false,
                        Data = null,
                    };
                }
                customer.Email = model.Email;
                customer.UserName = model.UserName;
                userProfile.FirstName = model.FirstName;
                userProfile.LastName = model.LastName;
                userProfile.PhoneNumber = model.PhoneNumber;
                userProfile.Email = model.Email;
                userProfile.AddressLine = model.AddressLine;
                userProfile.City = model.City;
                userProfile.State = model.State;
                userProfile.PostalCode = model.PostalCode;
                userProfile.Country = model.Country;

                await _userProfileRepository.Update(userProfile);
                await _customerRepository.Update(customer);
                await _unitOfWork.SaveChangesAsync();

                return new BaseResponse<CustomerDto>
                {
                    Message = "Customer updated successfully.",
                    Status = true,
                    Data = new CustomerDto
                    {
                        Id = customer.Id,
                        Email = customer.Email,
                        UserName = customer.UserName,
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
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<CustomerDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }
   

        public async Task<BaseResponse<ICollection<CustomerDto>>> GetAllAsync()
        {
            try 
            {
                var customer = await _customerRepository.GetAllAsync();
                if (customer == null)
                {
                    return new BaseResponse<ICollection<CustomerDto>>
                    {
                        Message ="Customer not found",
                        Status = false,
                        Data= null,
                    };
                }
                var listOfCustomer = customer.Select(a => new CustomerDto

                {

                    Id = a.Id,
                    Email = a.Email,
                    UserName = a.UserName,
                    UserProfile = new UserProfileDto
                    {
                        Id = a.User.Profile.Id,
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
                    },
                });
                return new BaseResponse<ICollection<CustomerDto>>
                {
                    Message = "Customers Found",
                    Status = true,
                    Data = listOfCustomer.ToList(),
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<ICollection<CustomerDto>>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }

        public async Task<BaseResponse<CustomerDto>> GetAsync(Guid id)
        {
            try
            {
                var customer = await _customerRepository.GetAsync(a => a.Id == id && a.IsDeleted == false);
                if (customer == null)
                {
                    return new BaseResponse<CustomerDto>
                    {
                        Message = $"Customer with id {id} does not exist",
                        Status = false,
                        Data = null,
                    };
                }
                return new BaseResponse<CustomerDto>
                {
                    Message = $"Customer with this id {id} exists",
                    Status = true,
                    Data = new CustomerDto
                    {

                        Id = customer.Id,
                        Email = customer.Email,
                        UserName = customer.UserName,
                        UserProfile = new UserProfileDto
                        {
                            Id = customer.User.Profile.Id,
                            UserId = customer.User.Profile.UserId,
                            Email = customer.User.Profile.Email,
                            FirstName = customer.User.Profile.FirstName,
                            LastName = customer.User.Profile.LastName,
                            PhoneNumber = customer.User.Profile.PhoneNumber,
                            AddressLine = customer.User.Profile.AddressLine,
                            City = customer.User.Profile.City,
                            State = customer.User.Profile.State,
                            PostalCode = customer.User.Profile.PostalCode,
                            Country = customer.User.Profile.Country
                        },
                    },
                };
            }
            

            catch (Exception ex)
            {
                return new BaseResponse<CustomerDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null,
                };
            }
        }
    }
}
