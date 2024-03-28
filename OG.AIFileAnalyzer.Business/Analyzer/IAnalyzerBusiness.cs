using OG.AIFileAnalyzer.Common.DTOs;

namespace OG.AIFileAnalyzer.Business.Analyzer
{
    public interface IAnalyzerBusiness
    {
        Task<AnalysisResponseDTO> Analyze(string base64Content);
    }
}
