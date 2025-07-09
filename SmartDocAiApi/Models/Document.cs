using System.ComponentModel.DataAnnotations;

namespace SmartDocAiApi.Models
{
    public class Document
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string FileName { get; set; }
        public string BlobUrl { get; set; }
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

    }
}