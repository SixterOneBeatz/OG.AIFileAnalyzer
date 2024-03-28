using Microsoft.AspNetCore.Components;
using OG.AIFileAnalyzer.Client.Pages.Analyzer;
using OG.AIFileAnalyzer.Client.Services.Historical;
using OG.AIFileAnalyzer.Common.Consts;
using OG.AIFileAnalyzer.Common.DTOs;
using OG.AIFileAnalyzer.Common.Entities;
using Radzen;
using Radzen.Blazor;
using System.Text.Json;

namespace OG.AIFileAnalyzer.Client.Pages.Historical
{
    public partial class Historical
    {

        [Inject]
        private IHistoricalService HistoricalService { get; set; }

        [Inject]
        private DialogService DialogService { get; set; }

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
                ActionType = ActionType.UserAction,
                Description = "Enter to Historical Module",
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

        async Task ShowAnalysisResult(LogEntity log)
        {
            FileDataDTO details = JsonSerializer.Deserialize<FileDataDTO>(log.Details);

            var data = await HistoricalService.GetAnalysisResult(details.SHA256);

            await DialogService.OpenAsync<DialogAnalysisDetail>("Analysis Result", new()
            {
                { "AnalysisData", data },
            });

            await Task.CompletedTask;
        }
    }
}
