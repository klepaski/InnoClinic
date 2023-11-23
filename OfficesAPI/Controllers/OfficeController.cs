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

        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Invalid image file");

            var filePath = Path.Combine("office-images", image.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }
            var imageUrl = $"http://localhost:5002/Office/GetImage/office-images%5C{image.FileName}";
            //var imageUrl = $"http://localhost:5002/Office/office-images\\{image.FileName}";
            return Ok(new { imageUrl });
        }

        [HttpGet("{imageUrl}")]
        public IActionResult GetImage(string imageUrl)
        {
            var imageBytes = System.IO.File.ReadAllBytes(imageUrl);
            return File(imageBytes, "image/jpeg");
        }
    }
}
