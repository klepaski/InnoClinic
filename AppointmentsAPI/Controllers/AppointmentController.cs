using AppointmentsAPI.Contracts.Requests;
using AppointmentsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentsAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
       
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _appointmentService.GetAll();
            return Ok(result);
        }

        //[Authorize(Roles = "Patient")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByPatient(int id)
        {
            var result = await _appointmentService.GetByPatient(id);
            return Ok(result);
        }

        //[Authorize(Roles = "Doctor")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByDoctor(int id)
        {
            var result = await _appointmentService.GetByDoctor(id);
            return Ok(result);
        }

        //[Authorize(Roles = "Patient, Receptionist")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentRequest req)
        {
            if (req == null) return BadRequest("Invalid data.");
            await _appointmentService.Create(req);
            return Ok("Appointment created.");
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Approve(int id)
        {
            await _appointmentService.Approve(id);
            return Ok("Approved.");
        }

        //[Authorize(Roles = "Receptionist")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _appointmentService.Delete(id);
            return Ok("Deleted.");
        }
    }
}