using Microsoft.EntityFrameworkCore;
using OG.AIFileAnalyzer.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Contexts
{
    public class AIFileAnalyzerDbContext(DbContextOptions<AIFileAnalyzerDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<LogEntity> Logs { get; set; }
    }
}
