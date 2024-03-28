using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OG.AIFileAnalyzer.Client.Services.Analyzer;
using OG.AIFileAnalyzer.Client.Services.Historical;
using OG.AIFileAnalyzer.Common.Consts;
using Radzen;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    public partial class DragNDrop
    {
        [Inject]
        private IAnalyzerService AnalyzerService { get; set; }

        [Inject]
        private IHistoricalService HistoricalService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

        async Task OnChange(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file != null)
            {
                var buffer = new byte[file.Size];
                await file.OpenReadStream().ReadAsync(buffer);

                var base64String = Convert.ToBase64String(buffer);

                await HistoricalService.Add(new()
                {
                    ActionType = ActionType.DocumentUpload,
                    Description = "Document loaded",
                });

                var data = await AnalyzerService.Analyze(base64String);

                await DialogService.OpenAsync<DialogAnalysisDetail>("Analysis Result", new()
                {
                    { "AnalysisData", data },
                });
            }
        }
    }
}
