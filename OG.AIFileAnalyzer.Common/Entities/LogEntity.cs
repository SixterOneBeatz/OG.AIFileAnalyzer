using OG.AIFileAnalyzer.Common.Consts;

namespace OG.AIFileAnalyzer.Common.Entities
{
    public class LogEntity
    {
        public int Id { get; set; }
        public ActionType ActionType { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
