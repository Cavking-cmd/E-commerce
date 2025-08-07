using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using E_commerce.Services.Interfaces;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            var result = await _productService.GetAll();
            return Ok(result);
        }

        [HttpGet("paginated")]
        public async Task<IActionResult> GetAllProductsPaginated([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var paginationParams = new PaginationParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _productService.GetAllPaginatedAsync(paginationParams);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(Guid id)
        {
            var result = await _productService.GetAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequestModel model)
        {
            var result = await _productService.CreateProduct(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string productName, UpdateProductModel model)
        {
            var result = await _productService.UpdateProduct(productName, model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var result = await _productService.DeleteProduct(id);
            if (result.Status)
            {
                return NoContent();
            }
            return NotFound(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchByTag([FromQuery] string tag)
        {
            if (string.IsNullOrWhiteSpace(tag))
                return BadRequest("Tag is required");

            var result = await _productService.SearchProductsByTagAsync(tag);
            if (!result.Status)
            {
                return BadRequest(result.Message);
            }
            return Ok(result.Data);
        }
    }
}
