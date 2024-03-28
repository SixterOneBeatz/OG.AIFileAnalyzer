using OG.AIFileAnalyzer.Common.Consts;
using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;
using OG.AIFileAnalyzer.Common.Helpers;
using OG.AIFileAnalyzer.Persistence.DataAccess.UnitOfWork;
using OG.AIFileAnalyzer.Persistence.Services.AzureAI;
using System.Text.Json;

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
            string hash = bytes.ComputeSHA256Hash();

            var file = await CheckForExistingAnalysis(hash);

            if (file != null)
            {
                result = file;
            }

            else
            {
                using (MemoryStream ms = new(bytes))
                {
                    var data = await _azureAIService.RunInvoiceAnalysis(ms);
                    result = new()
                    {
                        Data = data,
                        DocumentType = DocType.Invoice
                    };
                }

                if (!result.Data.ContainsKey("InvoiceId") && !result.Data.ContainsKey("InvoiceTotal"))
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
                    Description = "AI Analysis Runned",
                    Details = JsonSerializer.Serialize(new FileDataDTO { SHA256 = hash })
                });

                await SaveAnalysis(result, hash);

                await _unitOfWork.Complete();
            }

            return result;
        }

        public async Task<AnalysisResponseDTO> CheckForExistingAnalysis(string hash)
        {
            AnalysisResponseDTO result = null;

            var file = (await _unitOfWork.Repository<FileEntity>().GetAsync(x => x.SHA256 == hash, [(y) => y.Anaysis])).FirstOrDefault();

            if (file != null)
            {
                result = new()
                {
                    DocumentType = file.DocumentType,
                    Data = file.Anaysis.ToDictionary(x => x.Key, x => x.Value),
                };
            }

            return result;
        }

        public async Task SaveAnalysis(AnalysisResponseDTO file, string hash)
        {
            if (file != null)
            {
                FileEntity entity = new()
                {
                    SHA256 = hash,
                    DocumentType = file.DocumentType,
                    Anaysis = file.Data.Select(x => new FileAnaysisEntity
                    {
                        Key = x.Key,
                        Value = x.Value
                    }).ToList(),
                };

                await _unitOfWork.Repository<FileEntity>().AddEntity(entity);
                await _unitOfWork.Complete();
            }
        }
    }
}
