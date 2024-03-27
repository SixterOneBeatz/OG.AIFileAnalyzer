using OG.AIFileAnalyzer.Common.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OG.AIFileAnalyzer.Business.Analyzer
{
    public interface IAnalyzerBusiness
    {
        Task<AnalysisResponseDTO> Analyze(string base64Content);
    }
}
