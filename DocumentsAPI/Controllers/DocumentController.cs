using DocumentsAPI.Services;
using DocumentsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DocumentsAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;

        public DocumentController(IDocumentService documentService)
        {
            _documentService = documentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var documents = await _documentService.GetAll();
            return Ok(documents);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var document = await _documentService.GetById(id);
            if (document == null) return NotFound($"Document with id {id} not found.");
            return Ok(document);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Document document)
        {
            await _documentService.Create(document);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _documentService.Delete(id);
            return NoContent();
        }
    }
}