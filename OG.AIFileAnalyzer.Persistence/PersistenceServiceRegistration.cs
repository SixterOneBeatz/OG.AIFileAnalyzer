using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OG.AIFileAnalyzer.Persistence.DataAccess.Contexts;
using OG.AIFileAnalyzer.Persistence.DataAccess.UnitOfWork;
using OG.AIFileAnalyzer.Persistence.Services.AzureAI;
using OG.AIFileAnalyzer.Persistence.Services.Report;

namespace OG.AIFileAnalyzer.Persistence
{
    /// <summary>
    /// Helper class for registering persistence services.
    /// </summary>
    public static class PersistenceServiceRegistration
    {
        /// <summary>
        /// Adds persistence services to the specified IServiceCollection.
        /// </summary>
        public static void AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Add DbContext with SQL Server connection
            services.AddDbContext<AIFileAnalyzerDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("AIFileAnalyzerDB")));

            // Register transient services
            services.AddTransient<IAzureAIService, AzureAIService>();
            services.AddTransient<IReportService, ReportService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }
    }
}
