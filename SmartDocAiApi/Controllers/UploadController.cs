using Microsoft.AspNetCore.Mvc;
using SmartDocAiApi.Models;
using SmartDocAiApi.Services;

namespace SmartDocAiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UploadController(IDocumentService documentService) : ControllerBase
    {
        private readonly IDocumentService _documentService = documentService;

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] UploadDocumentRequest request)
        {
            try
            {
                if (request.File == null || request.File.Length == 0)
                {
                    return BadRequest("No file Uploaded");
                }

                var savedDocument = await _documentService.UploadDocumentsAsync(request.File);
                return Ok(savedDocument);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
    }
}