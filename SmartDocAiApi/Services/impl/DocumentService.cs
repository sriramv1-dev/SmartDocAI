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

            var bloburl = await _blobService.UploadAsync(file);
            var document = new Document
            {
                FileName = file.FileName,
                BlobUrl = bloburl
            };

            return await _repo.AddDocumentAsync(document);

        }
    }
}