using Microsoft.AspNetCore.Components;
using OG.AIFileAnalyzer.Common.DTOs;
using Radzen;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    /// <summary>
    /// Partial class representing the DialogAnalysisDetail component.
    /// </summary>
    public partial class DialogAnalysisDetail
    {
        /// <summary>
        /// Injected instance of the dialog service.
        /// </summary>
        [Inject]
        private DialogService DialogService { get; set; }

        /// <summary>
        /// Gets or sets the analysis data parameter.
        /// </summary>
        [Parameter]
        public AnalysisResponseDTO AnalysisData { get; set; }
    }
}
