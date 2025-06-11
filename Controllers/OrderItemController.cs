using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using E_commerce.Services.Interfaces;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemService _orderItemService;
        public OrderItemController(IOrderItemService orderItemService)
        {
            _orderItemService = orderItemService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrderItem([FromBody] CreateOrderItemRequestModel model)
        {
            var result = await _orderItemService.CreateOrderItemAsync(model);
            if (!result.Status)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderItem(Guid id)
        {
            var result = await _orderItemService.GetAsync(id);
            if (!result.Status)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderItems()
        {
            var result = await _orderItemService.GetAll();
            if (!result.Status)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateOrderItem([FromBody] UpdateOrderItemRequestModel model)
        {
            var result = await _orderItemService.UpdateOrderItem(model);
            if (!result.Status)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrderItem(Guid id)
        {
            var result = await _orderItemService.DeleteOrderItem(id);
            if (!result.Status)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }
    }
}
