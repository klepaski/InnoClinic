using Microsoft.AspNetCore.Mvc;
using Orchestrator.Services;
using JuliaChistyakovaPackage;

namespace Orchestrator.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class OrchestratorController : ControllerBase
    {
        private readonly IAuthService _authService;

        public OrchestratorController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var user = await _authService.CreateUser(req);
            if (user == null) return BadRequest("Can not create user.");

            var account = await _authService.CreateAccount(user, req);
            if (account == false) return BadRequest("Can not create account.");
            return Ok("Account created.");
        }
    }
}