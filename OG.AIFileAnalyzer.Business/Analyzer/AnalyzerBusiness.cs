using OG.AIFileAnalyzer.Common.Consts;
using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;
using OG.AIFileAnalyzer.Persistence.DataAccess.UnitOfWork;
using OG.AIFileAnalyzer.Persistence.Services.AzureAI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Business.Analyzer
{
    public class AnalyzerBusiness(IAzureAIService azureAIService, IUnitOfWork unitOfWork) : IAnalyzerBusiness
    {
        private readonly IAzureAIService _azureAIService = azureAIService;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        public async Task<AnalysisResponseDTO> Analyze(string base64Content)
        {
            AnalysisResponseDTO result;
            byte[] bytes = Convert.FromBase64String(base64Content);
            using (MemoryStream ms = new(bytes))
            {
                var data = await _azureAIService.RunInvoiceAnalysis(ms);
                result = new()
                {
                    Data = data,
                    DocumentType = DocType.Invoice
                };
            }

            if (!result.Data.ContainsKey("InvoiceNumer") && !result.Data.ContainsKey("InvoiceTotal"))
            {
                bytes = Convert.FromBase64String(base64Content);
                using (MemoryStream ms = new(bytes))
                {
                    var data = await _azureAIService.RunTextAnalysis(ms);
                    result = new()
                    {
                        Data = data,
                        DocumentType = DocType.GeneralText
                    };
                }
            }

            await _unitOfWork.Repository<LogEntity>().AddEntity(new()
            {
                ActionType = ActionType.IA,
                Description = "AI Analysis Runned"
            });

            await _unitOfWork.Complete();

            return result;
        }
    }
}
