using OG.AIFileAnalyzer.Common.DTOs;

namespace OG.AIFileAnalyzer.Client.Services.Analyzer
{
    public interface IAnalyzerService
    {
        Task<AnalysisResponseDTO> Analyze(string base64String);
    }
}
