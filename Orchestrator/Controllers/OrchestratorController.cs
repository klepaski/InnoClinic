using Microsoft.AspNetCore.Mvc;
using Orchestrator.Contracts.Requests;
using Orchestrator.Services;

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

            var account = await _authService.CreateAccount(req, user);
            if (account == null) return BadRequest("Can not create account.");
            return Ok(account);
        }
    }
}