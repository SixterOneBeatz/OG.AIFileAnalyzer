namespace OG.AIFileAnalyzer.Persistence.Services.AzureAI
{
    /// <summary>
    /// Service for interacting with Azure AI services.
    /// </summary>
    public interface IAzureAIService
    {
        /// <summary>
        /// Runs invoice analysis on the provided document.
        /// </summary>
        /// <param name="document">The document to analyze.</param>
        /// <returns>A dictionary containing analysis results.</returns>
        Task<Dictionary<string, string>> RunInvoiceAnalysis(MemoryStream document);

        /// <summary>
        /// Runs text analysis on the provided document.
        /// </summary>
        /// <param name="document">The document to analyze.</param>
        /// <returns>A dictionary containing analysis results.</returns>
        Task<Dictionary<string, string>> RunTextAnalysis(MemoryStream document);
    }
}
