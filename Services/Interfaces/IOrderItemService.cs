using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<BaseResponse<OrderItemDto>> CreateOrderItemAsync(CreateOrderItemRequestModel model);
        Task<BaseResponse<OrderItemDto>> GetAsync(Guid id);
        Task<BaseResponse<ICollection<OrderItemDto>>> GetAll();
        Task<BaseResponse<OrderItemDto>> UpdateOrderItem(UpdateOrderItemRequestModel model);
        Task<BaseResponse<bool>> DeleteOrderItem(Guid id);
    }
}
