using OG.AIFileAnalyzer.Common.DTOs;

namespace OG.AIFileAnalyzer.Client.Services.Analyzer
{
    public class AnalyzerService(HttpClient httpClient) : BaseService(httpClient), IAnalyzerService
    {

        /// <inheritdoc/>
        public async Task<AnalysisResponseDTO> Analyze(string base64String)
            => await PostAsync<AnalysisResponseDTO, string>("Analyzer/Analyze", base64String);
    }
}
