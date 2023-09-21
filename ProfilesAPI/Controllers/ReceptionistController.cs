using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Contracts.Requests;
using ProfilesAPI.Contracts.Responses;
using ProfilesAPI.Models;
using ProfilesAPI.Services;

namespace ProfilesAPI.Controllers
{
    //[Authorize(Roles = "Receptionist")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class ReceptionistController : ControllerBase
    {
        private readonly IReceptionistService _receptionistService;

        public ReceptionistController(IReceptionistService receptionistService)
        {
            _receptionistService = receptionistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<GetReceptionistResponse> receptionists = await _receptionistService.GetAll();
            return Ok(receptionists);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var receptionist = await _receptionistService.GetById(id);
            if (receptionist is null) return NotFound($"Receptionist with id {id} not found.");
            return Ok(receptionist);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReceptionistRequest receptionist)
        {
            var creatorName = User.Identity.Name;
            var result = await _receptionistService.Create(creatorName, receptionist);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateReceptionistRequest receptionist)
        {
            var updatorName = User.Identity.Name;
            var result = await _receptionistService.Update(updatorName, receptionist);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _receptionistService.Delete(id);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }
    }
}