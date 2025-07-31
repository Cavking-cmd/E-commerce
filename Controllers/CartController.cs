﻿using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        [HttpPost]
        public async Task<ActionResult<BaseResponse<CartController>>> Create([FromBody] CreateCartRequestModel model)
        {
            var result = await _cartService.CreateCart(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }


        [HttpGet("id")]
        public async Task<ActionResult<BaseResponse<CartController>>> GetById(Guid id)
        {
            var result = await _cartService.GetAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpGet]
        public async Task<ActionResult<BaseResponse<CartController>>> GetAll()
        {
            var result = await _cartService.GetAll();
            if(result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        
    }
}
