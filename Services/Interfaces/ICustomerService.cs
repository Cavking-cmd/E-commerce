using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<BaseResponse<CustomerDto>> CreateCustomer(CreateCustomerRequestModel model);
    }
}
