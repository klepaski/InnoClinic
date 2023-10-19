using AppointmentsAPI.Contracts.Requests;
using AppointmentsAPI.Models;
using AppointmentsAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net;

namespace AppointmentsAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ResultController : ControllerBase
    {
        private readonly IResultService _resultService;

        public ResultController(IResultService resultService)
        {
            _resultService = resultService;
        }

        //[Authorize(Roles = "Doctor, Patient")]
        [HttpGet("{appointmentId}")]
        public async Task<IActionResult> Get(int appointmentId)
        {
            var result = await _resultService.Get(appointmentId);
            return Ok(result);
        }

        //[Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateResultRequest req)
        {
            if (req == null) return BadRequest("Invalid data.");
            await _resultService.Create(req);
            return Ok("Result created.");
        }

        //[Authorize(Roles = "Doctor")]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateResultRequest req)
        {
            if (req == null) return BadRequest("Invalid data.");
            await _resultService.Update(req);
            return Ok("Result updated.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var bytes = await _resultService.DownloadPdf(id);
            return File(bytes, "application/pdf", "Result.pdf");
        }
    }
}