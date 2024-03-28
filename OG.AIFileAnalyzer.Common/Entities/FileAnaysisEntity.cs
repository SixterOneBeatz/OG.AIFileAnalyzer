using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Common.Entities
{
    public class FileAnaysisEntity : BaseEntity
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public int FileId { get; set; }
        public FileEntity File { get; set; }
    }
}
