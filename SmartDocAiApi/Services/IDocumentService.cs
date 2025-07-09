namespace SmartDocAiApi.Services
{
    public interface IDocumentService
    {
        Task<Models.Document> UploadDocumentsAsync(IFormFile file);
    }

}