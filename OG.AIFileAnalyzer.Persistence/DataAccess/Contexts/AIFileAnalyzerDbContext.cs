using Microsoft.EntityFrameworkCore;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Contexts
{
    public class AIFileAnalyzerDbContext(DbContextOptions<AIFileAnalyzerDbContext> dbContextOptions) : DbContext(dbContextOptions)
    {
        public DbSet<LogEntity> Logs { get; set; }
        public DbSet<FileEntity> Files { get; set; }
        public DbSet<FileAnaysisEntity> FileAnalyses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FileEntity>().HasIndex(x => x.SHA256).IsUnique();

            modelBuilder.Entity<FileAnaysisEntity>()
            .HasOne(e => e.File)
            .WithMany(e => e.Anaysis)
            .HasForeignKey(e => e.FileId);
        }
    }
}
