using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;
using OG.AIFileAnalyzer.Persistence.DataAccess.UnitOfWork;

namespace OG.AIFileAnalyzer.Business.Historical
{
    public class HistoricalBusiness(IUnitOfWork unitOfWork) : IHistoricalBusiness
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task AddHistorical(LogEntity entity)
        {
            await _unitOfWork.Repository<LogEntity>().AddEntity(entity);
            await _unitOfWork.Complete();
        }

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

        public async Task<List<LogEntity>> GetHistorical()
        {
            return await _unitOfWork.Repository<LogEntity>().GetAllAsync();
        }

        public async Task<HistoricalResultDTO> GetHistorical(HistoricalFilterDTO filter)
        {
            var (values, total) = await _unitOfWork.Repository<LogEntity>().GetAsync(filter.Skip, filter.Take);

            return new()
            {
                Rows = values,
                TotalRows = total
            };
        }
    }
}
