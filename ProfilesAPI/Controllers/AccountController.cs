using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Contracts.Requests;
using ProfilesAPI.Services;
using SharedModels;

namespace ProfilesAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet("{accountId}")]
        public async Task<IActionResult> ConfirmEmail(int accountId)
        {
            var result = await _accountService.ConfirmEmail(accountId);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest req)
        {
            var account = await _accountService.Create(req);
            if (account == null) return BadRequest("Someone already uses this email.");
            return Ok(account);
        }
    }
}
