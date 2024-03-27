using Microsoft.AspNetCore.Components;
using OG.AIFileAnalyzer.Client.Services.Historical;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    public partial class Analyzer
    {
        [Inject]
        private IHistoricalService HistoricalService { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await HistoricalService.Add(new()
            {
                ActionType = Common.Consts.ActionType.UserAction,
                Description = "Enter to Analyzer Module",
                Details = string.Empty
            });

            await base.OnInitializedAsync();
        }
    }
}
