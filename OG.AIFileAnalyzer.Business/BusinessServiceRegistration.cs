using Microsoft.Extensions.DependencyInjection;
using OG.AIFileAnalyzer.Business.Analyzer;
using OG.AIFileAnalyzer.Business.Historical;

namespace OG.AIFileAnalyzer.Business
{
    /// <summary>
    /// Static class responsible for registering business services.
    /// </summary>
    public static class BusinessServiceRegistration
    {
        /// <summary>
        /// Adds business services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to which the services are added.</param>
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IAnalyzerBusiness, AnalyzerBusiness>();
            services.AddScoped<IHistoricalBusiness, HistoricalBusiness>();
        }
    }
}
