namespace OG.AIFileAnalyzer.Common.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
