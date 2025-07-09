
using Microsoft.EntityFrameworkCore;

namespace SmartDocAiApi.Models
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Document> Documents { get; set; }

    }
}