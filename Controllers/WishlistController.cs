using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using E_commerce.Services.Interfaces;
using E_commerce.Core.Dtos.Request;
using E_commerce.Core.Dtos.WishlistDtos;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateWishlist()
        {
            var result = await _wishlistService.CreateAsync();
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllWishlists()
        {
            var result = await _wishlistService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWishlist(Guid id)
        {
            var result = await _wishlistService.GetByIdAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateWishlist([FromBody] UpdateWishlistRequestModel model)
        {
            var result = await _wishlistService.UpdateAsync(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWishlist(Guid id)
        {
            var result = await _wishlistService.DeleteAsync(id);
            if (result.Status)
            {
                return NoContent();
            }
            return NotFound(result);
        }
    }
}
