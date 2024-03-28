using OG.AIFileAnalyzer.Common.Consts;

namespace OG.AIFileAnalyzer.Common.Entities
{
    public class FileEntity : BaseEntity
    {
        public string SHA256 { get; set; }
        public DocType DocumentType { get; set; }

        public ICollection<FileAnaysisEntity> Anaysis { get; set; }
    }
}
