using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Common.DTOs
{
    public class HistoricalResultDTO
    {
        public List<LogEntity> Rows { get; set; }
        public int TotalRows { get; set; }
    }
}
