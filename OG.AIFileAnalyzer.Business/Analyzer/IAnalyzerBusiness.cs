using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Business.Analyzer
{
    public interface IAnalyzerBusiness
    {
        Task<AnalysisResponseDTO> Analyze(string base64Content);

        Task<AnalysisResponseDTO> CheckForExistingAnalysis(string hash);

        Task SaveAnalysis(AnalysisResponseDTO file, string hash);
    }
}
