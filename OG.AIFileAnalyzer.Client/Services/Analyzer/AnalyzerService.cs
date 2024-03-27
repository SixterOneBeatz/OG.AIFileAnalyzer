using OG.AIFileAnalyzer.Common.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace OG.AIFileAnalyzer.Client.Services.Analyzer
{
    public class AnalyzerService(HttpClient httpClient) : BaseService(httpClient), IAnalyzerService
    {
        public async Task<AnalysisResponseDTO> Analyze(string base64String)
            => await PostAsync<AnalysisResponseDTO, string>("Analyzer/Analyze", base64String);
    }
}
