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
        Task<Dictionary<string, string>> RunInvoiceAnalysis(MemoryStream document);
        Task<Dictionary<string, string>> RunTextAnalysis(MemoryStream document);
    }
}
