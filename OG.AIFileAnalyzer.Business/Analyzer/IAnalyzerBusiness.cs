using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Business.Analyzer
{
    /// <summary>
    /// Represents a business service for analysis operations.
    /// </summary>
    public interface IAnalyzerBusiness
    {
        /// <summary>
        /// Analyzes the provided content asynchronously.
        /// </summary>
        /// <param name="base64Content">The content to analyze encoded as a Base64 string.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the analysis response.</returns>
        Task<AnalysisResponseDTO> Analyze(string base64Content);

        /// <summary>
        /// Checks for existing analysis results for the specified hash.
        /// </summary>
        /// <param name="hash">The hash value used to check for existing analysis.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the analysis response if found; otherwise, null.</returns>
        Task<AnalysisResponseDTO> CheckForExistingAnalysis(string hash);

        /// <summary>
        /// Saves the analysis response for the specified hash.
        /// </summary>
        /// <param name="file">The analysis response to save.</param>
        /// <param name="hash">The hash value used to save the analysis response.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveAnalysis(AnalysisResponseDTO file, string hash);
    }
}
