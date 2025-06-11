using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<BaseResponse<CustomerDto>> CreateCustomer(CreateCustomerRequestModel model);
        Task<BaseResponse<ICollection<CustomerDto>>> GetAllAsync();
        Task<BaseResponse<CustomerDto>> GetAsync(Guid id);
        // The customer should have the update and the delet4e functions
    }
}
