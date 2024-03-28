using Microsoft.EntityFrameworkCore;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Contexts
{
    public class AIFileAnalyzerDbContext(DbContextOptions<AIFileAnalyzerDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<LogEntity> Logs { get; set; }
    }
}
