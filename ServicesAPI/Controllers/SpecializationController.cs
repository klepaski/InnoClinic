using Microsoft.AspNetCore.Authorization;
using ServicesAPI.Services;
using ServicesAPI.Models;
using Microsoft.AspNetCore.Mvc;
using ServicesAPI.Contracts.Requests;

namespace ServicesAPI.Controllers
{
    //[Authorize(Roles = "Receptionist")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class SpecializationController : ControllerBase
    {
        private readonly ISpecializationService _specializationService;

        public SpecializationController(ISpecializationService specializationService)
        {
            _specializationService = specializationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var specializations = await _specializationService.GetAll();
            return Ok(specializations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var service = await _specializationService.GetById(id);
            return Ok(service);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSpecializationRequest req)
        {
            var result = await _specializationService.Create(req);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateSpecializationRequest? req)
        {
            var result = await _specializationService.Update(req);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        [HttpPut("{specializationId}/{isActive}")]
        public async Task<IActionResult> ChangeStatus(int specializationId, bool isActive)
        {
            var result = await _specializationService.ChangeStatus(specializationId, isActive);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }
    }
}