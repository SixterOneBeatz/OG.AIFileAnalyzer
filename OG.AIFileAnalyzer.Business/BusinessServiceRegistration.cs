using Microsoft.Extensions.DependencyInjection;
using OG.AIFileAnalyzer.Business.Analyzer;
using OG.AIFileAnalyzer.Business.Historical;

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
