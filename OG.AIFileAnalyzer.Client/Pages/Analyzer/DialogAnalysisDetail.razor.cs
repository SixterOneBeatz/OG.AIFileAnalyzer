using Microsoft.AspNetCore.Components;
using OG.AIFileAnalyzer.Common.DTOs;
using Radzen;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    public partial class DialogAnalysisDetail
    {
        [Inject]
        private DialogService DialogService { get; set; }

        [Parameter]
        public AnalysisResponseDTO AnalysisData { get; set; }
    }
}
