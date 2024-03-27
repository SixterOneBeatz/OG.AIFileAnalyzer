using Microsoft.AspNetCore.Components;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    public partial class TextSummary
    {
        [Parameter]
        public Dictionary<string, string> Summaries { get; set; }
    }
}
