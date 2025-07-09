
namespace SmartDocAiApi.Repositories
{
    public interface IDocumentRepository
    {
        Task<Models.Document> AddDocumentAsync(Models.Document document);
    }
}