using Microsoft.AspNetCore.Components;
using OG.AIFileAnalyzer.Client.Services.Historical;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    /// <summary>
    /// Partial class representing the Analyzer component.
    /// </summary>
    public partial class Analyzer : ComponentBase
    {
        /// <summary>
        /// Injected instance of the historical service.
        /// </summary>
        [Inject]
        private IHistoricalService HistoricalService { get; set; }

        /// <summary>
        /// Overrides the base method to perform initialization asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            // Adds an entry to historical data indicating user action upon entering the Analyzer module.
            await HistoricalService.Add(new LogEntity
            {
                ActionType = Common.Consts.ActionType.UserAction,
                Description = "Enter to Analyzer Module",
            });

            await base.OnInitializedAsync();
        }
    }
}
