using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OG.AIFileAnalyzer.Client.Services.Analyzer;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    public partial class Analyzer
    {
        [Inject]
        private IAnalyzerService AnalyzerService { get;set; }

        async Task OnChange(InputFileChangeEventArgs e)
        {
            var file = e.File;

            if (file != null)
            {
                var buffer = new byte[file.Size];
                await file.OpenReadStream().ReadAsync(buffer);

                var base64String = Convert.ToBase64String(buffer);

                var data = await AnalyzerService.Analyze(base64String);
            }
        }
    }
}
