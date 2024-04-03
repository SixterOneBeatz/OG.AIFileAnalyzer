using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Client.Services.Historical
{
    /// <summary>
    /// Represents a service for historical data operations.
    /// </summary>
    public interface IHistoricalService
    {
        /// <summary>
        /// Retrieves historical data based on the specified filter.
        /// </summary>
        /// <param name="filter">The filter to apply to the historical data.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the historical data result.</returns>
        Task<HistoricalResultDTO> GetQueryable(HistoricalFilterDTO filter);

        /// <summary>
        /// Adds a log entity to the historical data.
        /// </summary>
        /// <param name="log">The log entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task Add(LogEntity log);

        /// <summary>
        /// Retrieves the analysis result for the specified hash.
        /// </summary>
        /// <param name="hash">The hash value used to retrieve the analysis result.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the analysis result.</returns>
        Task<AnalysisResponseDTO> GetAnalysisResult(string hash);

        /// <summary>
        /// Retrieves a report as a stream.
        /// </summary>
        /// <param name="filter">The filter to apply to the historical data.</param>
        /// <returns>A task representing the asynchronous operation. The task result contains the report stream.</returns>
        Task<Stream> GetReport(HistoricalFilterDTO filter);
    }
}
