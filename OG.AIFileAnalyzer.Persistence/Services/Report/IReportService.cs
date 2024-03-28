using OG.AIFileAnalyzer.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Persistence.Services.Report
{
    public interface IReportService
    {
        MemoryStream ExportToExcel(List<LogEntity> logs);
    }
}
