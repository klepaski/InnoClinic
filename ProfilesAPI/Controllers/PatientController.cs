using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Contracts.Requests;
using ProfilesAPI.Contracts.Responses;
using ProfilesAPI.Services;
using ProfilesAPI.Models;

namespace ProfilesAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PatientController : Controller
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            List<GetAllPatientsResponse> patients = await _patientService.GetAll();
            return Ok(patients);
        }

        //[Authorize(Roles = "Patient, Doctor, Receptionist")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var patient = await _patientService.GetById(id);
            if (patient is null) return NotFound($"Patient with id {id} not found.");
            return Ok(patient);
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpPost]
        public async Task<IActionResult> CreateByAdmin([FromBody] CreatePatientByAdminRequest patient)
        {
            var result = await _patientService.CreateByAdmin(patient);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _patientService.Delete(id);
            return result.Success ?
                Ok(result.Message) :
                NotFound(result.Message);
        }

        //[Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientRequest patient)
        {
            var result = await _patientService.Create(patient);
            return result.Success ?
                Ok(result.Message) :
                Ok(result.FoundPatient);
        }

        //[Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> CreatePatientWithNewAccount([FromBody] CreatePatientRequest patient)
        {
            var result = await _patientService.CreatePatientWithNewAccount(patient);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        //[Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> LinkPatientToAccount([FromBody] Patient patient, Account account)
        {
            var result = await _patientService.LinkPatientToAccount(patient, account);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }

        //[Authorize(Roles = "Receptionist, Patient")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdatePatientRequest patient)
        {
            var updatorName = User.Identity.Name;
            var result = await _patientService.Update(updatorName, patient);
            return result.Success ?
                Ok(result.Message) :
                BadRequest(result.Message);
        }
    }
}
