using OG.AIFileAnalyzer.Common.Consts;

namespace OG.AIFileAnalyzer.Common.Entities
{
    public class LogEntity : BaseEntity
    {
        public ActionType ActionType { get; set; }
        public string Description { get; set; }
        public string Details { get; set; }
    }
}
