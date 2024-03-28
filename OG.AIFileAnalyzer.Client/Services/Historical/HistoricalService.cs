using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Client.Services.Historical
{
    public class HistoricalService(HttpClient httpClient) : BaseService(httpClient), IHistoricalService
    {
        /// <inheritdoc/>
        public async Task Add(LogEntity log)
        {
            await PostAsync("Historical/Add", log);
        }

        /// <inheritdoc/>
        public async Task<AnalysisResponseDTO> GetAnalysisResult(string hash)
        {
            return await GetAsync<AnalysisResponseDTO>($"Historical/GetAnalysisResult/{hash}");
        }

        /// <inheritdoc/>
        public async Task<HistoricalResultDTO> GetQueryable(HistoricalFilterDTO filter)
        {
            return await PostAsync<HistoricalResultDTO, HistoricalFilterDTO>("Historical/GetQueryable", filter);
        }

        /// <inheritdoc/>
        public async Task<Stream> GetReport()
        {
            return await GetAsync("Historical/GetReport");
        }
    }
}
