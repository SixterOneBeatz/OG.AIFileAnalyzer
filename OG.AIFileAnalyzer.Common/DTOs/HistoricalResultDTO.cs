using OG.AIFileAnalyzer.Common.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Common.DTOs
{
    public class HistoricalResultDTO
    {
        public List<LogEntity> Rows { get; set; }
        public int TotalRows { get; set; }
    }
}
