namespace OG.AIFileAnalyzer.Persistence.Services.AzureAI
{
    public interface IAzureAIService
    {
        Task<Dictionary<string, string>> RunInvoiceAnalysis(MemoryStream document);
        Task<Dictionary<string, string>> RunTextAnalysis(MemoryStream document);
    }
}
