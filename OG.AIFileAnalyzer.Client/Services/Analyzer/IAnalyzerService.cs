using OG.AIFileAnalyzer.Common.DTOs;

namespace OG.AIFileAnalyzer.Client.Services.Analyzer
{
    /// <summary>
    /// Represents a service for data analysis.
    /// </summary>
    public interface IAnalyzerService
    {
        /// <summary>
        /// Analyzes the provided data asynchronously.
        /// </summary>
        /// <param name="base64String">The data to analyze encoded as a Base64 string.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the analysis response.</returns>
        Task<AnalysisResponseDTO> Analyze(string base64String);
    }
}
