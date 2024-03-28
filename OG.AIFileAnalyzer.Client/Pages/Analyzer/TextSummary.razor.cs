using Microsoft.AspNetCore.Components;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    /// <summary>
    /// Partial class representing the TextSummary component.
    /// </summary>
    public partial class TextSummary
    {
        /// <summary>
        /// Gets or sets the summaries parameter.
        /// </summary>
        [Parameter]
        public Dictionary<string, string> Summaries { get; set; }
    }
}
