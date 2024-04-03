using OG.AIFileAnalyzer.Common.Consts;

namespace OG.AIFileAnalyzer.Common.DTOs
{
    public class HistoricalFilterDTO : BaseFilterDTO
    {
        public ActionType Action { get; set; }
        public string Description { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
