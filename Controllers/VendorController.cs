using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IVendorService _vendorService;
        public VendorController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        [HttpPost]
        public async Task<ActionResult<BaseResponse<VendorDto>>> CreateVendor([FromBody] CreateVendorRequestModel model)
        {
            var result = await _vendorService.CreateVendor(model);
            if (result.Status == true)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<VendorDto>>> GetVendor(Guid id)
        {
            var result = await _vendorService.GetAsync(id);
            if (result.Status == true)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse<VendorDto>>> GetAllVendors()
        {
            var result = await _vendorService.GetAll();
            if (result.Status == true)
            {
                return Ok(result);
            }
            return NotFound(result);





        }

    }
}
