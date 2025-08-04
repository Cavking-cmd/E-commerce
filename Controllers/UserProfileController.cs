using E_commerce.Core.Dtos.Request;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace E_commerce.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _userProfileService.GetAll();
            if (!response.Status)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var response = await _userProfileService.GetById(id);
            if (!response.Status)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}
