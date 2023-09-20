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

        //[Authorize(Roles = "Doctor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctorByDoctor(int id)
        {
            var doctor = await _doctorService.GetDoctorByDoctor(id);
            if (doctor == null) return NotFound($"Doctor with id {id} not found.");
            return Ok(doctor);
        }

        //[Authorize(Roles = "Patient")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<GetDoctorsResponse> doctors = await _doctorService.GetAll();
            return Ok(doctors);
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDoctorRequest doctor)
        {
            var receptionist = User.Identity.Name;
            var result = await _doctorService.Create(receptionist, doctor);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }
    }
}