using OG.AIFileAnalyzer.Common.Consts;

namespace OG.AIFileAnalyzer.Common.DTOs
{
    public class AnalysisResponseDTO
    {
        public Dictionary<string, string> Data { get; set; }
        public DocType DocumentType { get; set; }
    }
}
