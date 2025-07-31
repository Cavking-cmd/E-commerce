using Microsoft.AspNetCore.Mvc;
using E_commerce.Services.Interfaces;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Entities;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;

        public OrderController(IOrderService orderService, IPaymentService paymentService)
        {
            _orderService = orderService;
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestModel model)
        {
            var result = await _orderService.CreateOrderAsync(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return Ok(result);
        }

        [HttpPost("payment/process")]
        public async Task<IActionResult> ProcessPaymentInitiatePaymentAsync(Order order)
        {
            var result = await _paymentService.InitiatePaymentAsync(order);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
