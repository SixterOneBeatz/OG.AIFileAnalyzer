using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Business.Historical
{
    public interface IHistoricalBusiness
    {
        Task<HistoricalResultDTO> GetHistorical(HistoricalFilterDTO filter);
        Task AddHistorical(LogEntity entity);
        Task<AnalysisResponseDTO> GetAnalysisResult(string hash);
    }
}
