using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;
using E_commerce.Repositories.Interfaces;
using E_commerce.Services.Interfaces;

namespace E_commerce.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseResponse<OrderDto>> CreateOrderAsync(CreateOrderRequestModel model)
        {
            try
            {
                if(Validator.CheckNull(model))
                {
                    return  new BaseResponse<OrderDto>
                    {
                        Message = "Order Model can't be null",
                        Status = false,
                        Data = null
                    };
                }
                if(Validator.CheckNegativeOrZero(model.TotalPrice))
                {
                    return new BaseResponse<OrderDto>
                    {
                        Message = " The total price can not be negative or zero",
                        Status = false,
                        Data = null
                    };
                }
                var exist =  await _orderRepository.CheckAsync(a => a.Name == model.Name);
                if (exist)
                {
                    return new BaseResponse<OrderDto>
                    {
                        Message="Order already exist",
                        Status = false,
                        Data = null
                    };

                }
                var order = new Order
                {
                    Name = model.Name,
                    OrderDate = model.OrderDate,
                    TotalPrice = model.TotalPrice,
                };
                await _orderRepository.CreateAsync(order);
                await _unitOfWork.SaveChangesAsync();
                return new BaseResponse<OrderDto>
                {
                    Message ="Order Created successfully",
                    Status = true,
                    Data = new OrderDto
                    {
                        Name=model.Name,
                        OrderDate = model.OrderDate,
                        TotalPrice = model.TotalPrice,
                    }
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderDto>
                {
                    Message = ex.Message,
                    Status = false,
                    Data = null
                };
            }
            
        }

        public Task<ICollection<BaseResponse<OrderDto>>> GetAllOrdersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<OrderDto>> GetOrderAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
