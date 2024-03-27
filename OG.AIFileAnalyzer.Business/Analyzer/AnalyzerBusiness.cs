using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Persistence.Services.AzureAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Business.Analyzer
{
    public class AnalyzerBusiness(IAzureAIService azureAIService) : IAnalyzerBusiness
    {
        private readonly IAzureAIService _azureAIService = azureAIService;
        public async Task<AnalysisResponseDTO> Analyze(string base64Content)
        {
            AnalysisResponseDTO result;
            byte[] bytes = Convert.FromBase64String(base64Content);
            using (MemoryStream ms = new(bytes))
            {
                result = await _azureAIService.RunInvoiceAnalysis(ms);
            }

            if (!result.Data.ContainsKey("InvoiceNumer") && !result.Data.ContainsKey("InvoiceTotal"))
            {
                bytes = Convert.FromBase64String(base64Content);
                using (MemoryStream ms = new(bytes))
                {
                    result = await _azureAIService.RunTextAnalysis(ms);
                }
            }

            return result;
        }
    }
}
