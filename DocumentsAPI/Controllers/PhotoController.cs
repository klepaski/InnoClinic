using DocumentsAPI.Services;
using DocumentsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PhotoController : ControllerBase
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var photos = await _photoService.GetAll();
            return Ok(photos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var photo = await _photoService.GetById(id);
            if (photo == null) return NotFound($"Photo with id {id} not found.");
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Photo photo)
        {
            await _photoService.Create(photo);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _photoService.Delete(id);
            return NoContent();
        }
    }
}