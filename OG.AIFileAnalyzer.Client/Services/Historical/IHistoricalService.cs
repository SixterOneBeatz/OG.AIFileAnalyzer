using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Client.Services.Historical
{
    public interface IHistoricalService
    {
        Task<HistoricalResultDTO> GetQueryable(HistoricalFilterDTO filter);
        Task Add(LogEntity log);
        Task<AnalysisResponseDTO> GetAnalysisResult(string hash);
    }
}
