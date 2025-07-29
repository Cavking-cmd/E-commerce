using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using E_commerce.Services.Interfaces;
using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;
        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReview([FromBody] ReviewDto model)
        {
            var result = await _reviewService.CreateReviewAsync(model);
            if (!result.Status)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetReview(Guid id)
        {
            var result = await _reviewService.GetReviewAsync(id);
            if (!result.Status)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var result = await _reviewService.GetAllReviewsAsync();
            if (!result.Status)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateReview([FromBody] ReviewDto model)
        {
            var result = await _reviewService.UpdateReviewAsync(model);
            if (!result.Status)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(Guid id)
        {
            var result = await _reviewService.DeleteReviewAsync(id);
            if (!result.Status)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }
    }
}
