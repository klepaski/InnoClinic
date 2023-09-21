using Microsoft.AspNetCore.Mvc;
using AuthAPI.Contracts.Requests;
using AuthAPI.Services;

namespace AuthAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            if (req is null) return BadRequest("Invalid request.");
            var result = await _authService.Login(req);
            return result.Success ?
                Ok(result) :
                Unauthorized(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            if (req is null) return BadRequest("Invalid request.");
            var result = await _authService.Register(req);
            return result.Success ?
                Ok(result.NewUser) :
                Unauthorized(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest req)
        {
            if (req is null) return BadRequest("Invalid request.");
            var result = await _authService.Refresh(req);
            return result.Success ?
                Ok(result) :
                Unauthorized(result.Message);
        }
    }
}