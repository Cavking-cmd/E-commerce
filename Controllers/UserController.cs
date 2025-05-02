using E_commerce.AuthService;
using E_commerce.Core.Dtos.UserDtos;
using E_commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;

        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }
        [Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel model)
        {
            var result = await _userService.LoginAsync(model);
            if (result.Status && result.Data != null)
            {
                string token = _authService.GenerateToken(result.Data);
                return Ok(new
                {
                    Message = result.Message,
                    Token = token,
                    User = result.Data
                });
            }

            return Unauthorized(result);
        }
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.DeleteAsync(id);
            if (result)
            {
                return NoContent();
            }

            return NotFound(new { Message = "User not found." });
        }
        [Authorize]
        [HttpPut("{email}")]
        public async Task<IActionResult> Update([FromRoute] string email, [FromBody] LoginRequestModel request)
        {
            var result = await _userService.Update(email, request);
            if (result.Status)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }
    }
}
