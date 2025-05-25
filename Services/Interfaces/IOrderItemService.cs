using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<BaseResponse<OrderItemDto>> CreateOrderItemAsync(CreateOrderItemRequestModel model);
    }
}
