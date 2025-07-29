using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Services.Interfaces
{
    public interface IOrderService
    {
        Task<BaseResponse<OrderDto>> CreateOrderAsync(CreateOrderRequestModel model ,Guid customerId);
        Task<ICollection<BaseResponse<OrderDto>>> GetAllOrdersByCustomerIdAsync(Guid customerId);
        Task<BaseResponse<OrderDto>> GetOrderByIdAsync(Guid id);
       
    }
}
