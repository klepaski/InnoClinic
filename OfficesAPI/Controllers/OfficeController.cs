using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficesAPI.Contracts.Requests;
using OfficesAPI.Models;
using OfficesAPI.Services;

namespace OfficesAPI.Controllers
{
    //[Authorize(Roles = "Receptionist")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class OfficeController : ControllerBase
    {
        private readonly IOfficeService _officeService;

        public OfficeController(IOfficeService officeService)
        {
            _officeService = officeService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var office = await _officeService.GetById(id);
            if (office == null) return NotFound($"Office with id {id} not found.");
            return Ok(office);
        }

        [HttpGet]
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetAll()
        {
            List<Office> offices = await _officeService.GetAll();
            return Ok(offices);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _officeService.Delete(id);
            return result.Success ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOfficeRequest office)
        {
            var result = await _officeService.Create(office);
            return result.Success ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOfficeRequest office)
        {
            var result = await _officeService.Update(office);
            return result.Success ?
                Ok(result) :
                BadRequest(result);
        }

        [HttpPut("{id}/{status}")]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            var result = await _officeService.ChangeStatus(id, status);
            return result.Success ?
                Ok(result) :
                BadRequest(result);
        }
    }
}
