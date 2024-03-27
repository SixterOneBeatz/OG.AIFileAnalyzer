using OG.AIFileAnalyzer.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Persistence.Services.AzureAI
{
    public interface IAzureAIService
    {
        Task<AnalysisResponseDTO> RunInvoiceAnalysis(MemoryStream document);
        Task<AnalysisResponseDTO> RunTextAnalysis(MemoryStream document);
    }
}
