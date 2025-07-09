using System.ComponentModel.DataAnnotations;

namespace SmartDocAiApi.Models
{
    public class UploadDocumentRequest
    {
        [Required]
        public IFormFile File { get; set; }
    }
}