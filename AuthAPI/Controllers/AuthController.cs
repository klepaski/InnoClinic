using Microsoft.AspNetCore.Mvc;
using AuthAPI.Contracts.Requests;
using AuthAPI.Contracts.Responses;
using AuthAPI.Services;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Security.Claims;

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
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (request is null) return BadRequest("Invalid client request");
            var result = await _authService.Login(request);
            return result.Success ?
                Ok(result) :
                Unauthorized(result);
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (request is null)
                return BadRequest("Invalid client request");
            var result = await _authService.Register(request);
            return result.Success ?
                Ok(result) :
                Unauthorized(result);
        }

        [HttpPost]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            if (request is null)
                return BadRequest("Invalid client request");
            var result = await _authService.Refresh(request);
            return result.Success ?
                Ok(result) :
                Unauthorized(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRole([FromHeader] string authorization)
        {
            return Ok(_authService.GetRole(authorization.Remove(0, 7)));
        }

    }
}