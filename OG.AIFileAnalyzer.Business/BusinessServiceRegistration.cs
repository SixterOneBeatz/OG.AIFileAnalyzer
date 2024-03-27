using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OG.AIFileAnalyzer.Business.Analyzer;
using OG.AIFileAnalyzer.Business.Historical;
using OG.AIFileAnalyzer.Persistence;

namespace OG.AIFileAnalyzer.Business
{
    public static class BusinessServiceRegistration
    {
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IAnalyzerBusiness, AnalyzerBusiness>();
            services.AddScoped<IHistoricalBusiness, HistoricalBusiness>();
        }
    }
}
