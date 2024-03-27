using OG.AIFileAnalyzer.Common.DTOs;
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

        public async Task<HistoricalResultDTO> GetQueryable(HistoricalFilterDTO filter)
        {
            return await PostAsync<HistoricalResultDTO, HistoricalFilterDTO>("Historical/GetQueryable", filter);
        }
    }
}
