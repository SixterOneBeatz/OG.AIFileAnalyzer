using OG.AIFileAnalyzer.Common.Entities;
using System.Net.Http;

namespace OG.AIFileAnalyzer.Client.Services.Historical
{
    public class HistoricalService(HttpClient httpClient) : BaseService(httpClient), IHistoricalService
    {
        public async Task Add(LogEntity log)
        {
            await PostAsync("Historical/Add", log);
        }

        public async Task<List<LogEntity>> GetAll()
        {
            return await GetAsync<List<LogEntity>>("Historical/GetAll");
        }
    }
}
