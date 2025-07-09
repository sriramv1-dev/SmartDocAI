using SmartDocAiApi.Models;

namespace SmartDocAiApi.Repositories.impl
{
    public class DocumentRepository(AppDbContext context) : IDocumentRepository
    {
        private readonly AppDbContext _context = context;
        public async Task<Document> AddDocumentAsync(Document document)
        {

            _context.Add(document);
            await _context.SaveChangesAsync();

            return document;
        }
    }
}