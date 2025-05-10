using E_commerce.Core.Dtos;
using E_commerce.Core.Dtos.Request;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]   
        public async Task<ActionResult<BaseResponse<CustomerDto>>> Create([FromBody] CreateCustomerRequestModel model)
        {
            var result = await _customerService.CreateCustomer(model);
            if (result.Status)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
        [HttpGet]
        public async Task<ActionResult<BaseResponse<CustomerController>>> GetAllCustomers()
        {
            var result = await _customerService.GetAllAsync();
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<BaseResponse<CustomerDto>>> GetCustomer(Guid id)
        {
            var result = await _customerService.GetAsync(id);
            if (result.Status)
            {
                return Ok(result);
            }
            return NotFound(result);
        }
    }
}
