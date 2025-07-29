using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemService _cartItemService;
        public CartItemController(ICartItemService cartItemService)
        {
            _cartItemService = cartItemService;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<CartItemController>>> Create([FromBody] CreateCartItemRequestModel model)
        {
            var result = await _cartItemService.CreateCartItem(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<CartItemController>>> GetAll()
        {
            var result = await _cartItemService.GetAll();
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet("Id")]
        public async Task<ActionResult<BaseResponse<CartItemController>>> GetById(Guid id)
        {
            var result = await _cartItemService.GetAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPut("Id")]
        public async Task<ActionResult<BaseResponse<CartItemController>>> UpdateCart([FromBody] UpdateCartItemRequestModel model)
        {
            var result = await _cartItemService.UpdateCartItem(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("Id")]
        public async Task<ActionResult<BaseResponse<CartItemController>>> DeleteCartItem(Guid id)
        {
            var result = await _cartItemService.DeleteCartItem(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
