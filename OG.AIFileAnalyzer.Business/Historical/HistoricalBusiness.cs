using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;
using OG.AIFileAnalyzer.Persistence.DataAccess.UnitOfWork;
using OG.AIFileAnalyzer.Persistence.Services.Report;

namespace OG.AIFileAnalyzer.Business.Historical
{
    public class HistoricalBusiness(IUnitOfWork unitOfWork, IReportService reportService) : IHistoricalBusiness
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IReportService _reportService = reportService;

        public async Task AddHistorical(LogEntity entity)
        {
            await _unitOfWork.Repository<LogEntity>().AddEntity(entity);
            await _unitOfWork.Complete();
        }

        /// <inheritdoc/>
        public async Task<AnalysisResponseDTO> GetAnalysisResult(string hash)
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

        /// <inheritdoc/>
        public async Task<List<LogEntity>> GetHistorical()
        {
            return await _unitOfWork.Repository<LogEntity>().GetAllAsync();
        }

        /// <inheritdoc/>
        public async Task<HistoricalResultDTO> GetHistorical(HistoricalFilterDTO filter)
        {
            var (values, total) = await _unitOfWork.Repository<LogEntity>().GetAsync(filter.Skip, filter.Take);

            return new()
            {
                Rows = values,
                TotalRows = total
            };
        }

        /// <inheritdoc/>
        public async Task<MemoryStream> GetReport()
        {
            var logs = await GetHistorical();
            return _reportService.ExportToExcel(logs);
        }
    }
}
