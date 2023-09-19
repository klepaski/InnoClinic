using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Contracts.Requests;
using ProfilesAPI.Contracts.Responses;
using ProfilesAPI.Models;
using ProfilesAPI.Services;

namespace ProfilesAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

        //[Authorize(Roles = "Patient")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<GetDoctorResponse> doctors = await _doctorService.GetAll();
            return Ok(doctors);
        }

        //[Authorize(Roles = "Doctor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorByDoctor(int id)
        {
            var doctor = await _doctorService.GetDoctorByDoctor(id);
            if (doctor == null) return NotFound($"Doctor with id {id} not found.");
            return Ok(doctor);
        }

        //[Authorize(Roles = "Patient")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorByPatient(int id)
        {
            var doctor = await _doctorService.GetDoctorByPatient(id);
            if (doctor == null) return NotFound($"Doctor with id {id} not found.");
            return Ok(doctor);
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDoctorRequest doctor)
        {
            var creatorName = User.Identity.Name;
            var result = await _doctorService.Create(creatorName, doctor);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        //[Authorize(Roles = "Receptionist, Doctor")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateDoctorRequest doctor)
        {
            var updatorName = User.Identity.Name;
            var result = await _doctorService.Update(updatorName, doctor);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpPut]
        public async Task<IActionResult> ChangeStatus(int id, string status)
        {
            var result = await _doctorService.ChangeStatus(id, status);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }
    }
}