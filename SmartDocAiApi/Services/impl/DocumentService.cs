using SmartDocAiApi.Models;
using SmartDocAiApi.Repositories;

namespace SmartDocAiApi.Services.impl
{
    public class DocumentService(
        IBlobStorageService blobService,
        IDocumentRepository repo
    ) : IDocumentService
    {

        private readonly IBlobStorageService _blobService = blobService;
        private readonly IDocumentRepository _repo = repo;
        public async Task<Document> UploadDocumentsAsync(IFormFile file)
        {

            try
            {
                var blobUrl = await _blobService.UploadAsync(file);
                var document = new Document
                {
                    FileName = file.FileName,
                    BlobUrl = blobUrl
                };

                return await _repo.AddDocumentAsync(document);
            }
            catch (Exception ex)
            {
                // Log the error - replace with your logger if available
                Console.WriteLine($"Error uploading document: {ex.Message}");
                // Optionally, you can rethrow or handle the error gracefully
                throw;
            }

        }
    }
}