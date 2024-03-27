using OG.AIFileAnalyzer.Business.Analyzer;
using OG.AIFileAnalyzer.Common.Entities;
using OG.AIFileAnalyzer.Persistence.DataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<List<LogEntity>> GetHistorical()
        {
            return await _unitOfWork.Repository<LogEntity>().GetAllAsync();
        }
    }
}
