using OG.AIFileAnalyzer.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Business.Historical
{
    public interface IHistoricalBusiness
    {
        Task<List<LogEntity>> GetHistorical();
        Task AddHistorical(LogEntity entity);
    }
}
