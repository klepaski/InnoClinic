using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Contracts.Requests;
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

        //[Authorize(Roles = "Patient, Receptionist")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _doctorService.GetAll();
            return Ok(doctors);
        }

        //Patient: + Services according to specialization
        //[Authorize(Roles = "Doctor, Patient, Receptionist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var doctor = await _doctorService.GetById(id);
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

        //[Authorize(Roles = "Receptionist, Patient")]
        [HttpGet]
        public async Task<IActionResult> Search(string name)
        {
            var matchingDoctors = await _doctorService.Search(name);
            return Ok(matchingDoctors);
        }

        //[Authorize(Roles = "Receptionist, Patient")]
        [HttpGet]
        public async Task<IActionResult> FilterBySpecialization(int specializationId)
        {
            var matchingDoctors = await _doctorService.FilterBySpecialization(specializationId);
            return Ok(matchingDoctors);
        }

        //[Authorize(Roles = "Receptionist, Patient")]
        [HttpGet]
        public async Task<IActionResult> FilterByOffice(int officeId)
        {
            var matchingDoctors = await _doctorService.FilterByOffice(officeId);
            return Ok(matchingDoctors);
        }
    }
}