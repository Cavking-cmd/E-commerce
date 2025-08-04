using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using E_commerce.Services.Interfaces;
using E_commerce.Core.Dtos.Request;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSubCategory([FromBody] CreateSubCategoryRequestModel model)
        {
            var result = await _subCategoryService.CreateSubCategory(model);
            if (!result.Status)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSubCategory(Guid id)
        {
            var result = await _subCategoryService.GetAsync(id);
            if (!result.Status)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSubCategories()
        {
            var result = await _subCategoryService.GetAll();
            if (!result.Status)
                return NotFound(result.Message);
            return Ok(result.Data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateSubCategory([FromBody] UpdateSubCategoryRequestModel model)
        {
            var result = await _subCategoryService.UpdateSubCategory(model);
            if (!result.Status)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubCategory(Guid id)
        {
            var result = await _subCategoryService.DeleteSubCategory(id);
            if (!result.Status)
                return BadRequest(result.Message);
            return Ok(result.Data);
        }
    }
}
