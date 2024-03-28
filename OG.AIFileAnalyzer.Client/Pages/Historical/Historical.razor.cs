using Microsoft.AspNetCore.Components;
using OG.AIFileAnalyzer.Client.Services.Historical;
using OG.AIFileAnalyzer.Common.Entities;
using Radzen;
using Radzen.Blazor;

namespace OG.AIFileAnalyzer.Client.Pages.Historical
{
    public partial class Historical
    {

        [Inject]
        private IHistoricalService HistoricalService { get; set; }

        private const int SKIP = 0;
        private const int TAKE = 15;

        ODataEnumerable<LogEntity> logs;
        RadzenDataGrid<LogEntity> logsGrid;
        bool isLoading;
        int count;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            await HistoricalService.Add(new()
            {
                ActionType = Common.Consts.ActionType.UserAction,
                Description = "Enter to Historical Module",
                Details = string.Empty
            });
        }

        async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            var query = await HistoricalService.GetQueryable(new()
            {
                Skip = args.Skip ?? SKIP,
                Take = args.Top ?? TAKE,
            });

            count = query.TotalRows;

            logs = query.Rows.AsODataEnumerable();

            isLoading = false;
        }
    }
}
