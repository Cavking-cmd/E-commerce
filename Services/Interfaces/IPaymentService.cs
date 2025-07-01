using E_commerce.Core.Dtos;
using E_commerce.Core.Entities;

namespace E_commerce.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<BaseResponse<string>> InitiatePaymentAsync(Order order);
    }

}
