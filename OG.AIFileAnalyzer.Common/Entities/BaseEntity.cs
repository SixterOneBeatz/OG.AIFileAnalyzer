using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Common.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
