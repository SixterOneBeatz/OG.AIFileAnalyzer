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
    /// <summary>
    /// Partial class representing the Historical component.
    /// </summary>
    public partial class Historical : ComponentBase
    {
        /// <summary>
        /// Injected instance of the historical service.
        /// </summary>
        [Inject]
        private IHistoricalService HistoricalService { get; set; }

        /// <summary>
        /// Injected instance of the dialog service.
        /// </summary>
        [Inject]
        private DialogService DialogService { get; set; }

        /// <summary>
        /// Injected instance of the navigation manager.
        /// </summary>
        [Inject]
        private NavigationManager NavigationManager { get; set; }

        /// <summary>
        /// Represents the number of records to skip when loading data.
        /// </summary>
        private const int SKIP = 0;

        /// <summary>
        /// Represents the number of records to take when loading data.
        /// </summary>
        private const int TAKE = 15;

        /// <summary>
        /// Enumerable collection of log entities.
        /// </summary>
        private ODataEnumerable<LogEntity> logs;

        /// <summary>
        /// Data grid component for displaying log entities.
        /// </summary>
        private RadzenDataGrid<LogEntity> logsGrid;

        /// <summary>
        /// Indicates whether data is being loaded.
        /// </summary>
        private bool isLoading;

        /// <summary>
        /// The total count of log entities.
        /// </summary>
        private int count;

        /// <summary>
        /// Overrides the base method to perform initialization asynchronously.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            // Adds an entry to historical data indicating user action upon entering the Historical module.
            await HistoricalService.Add(new LogEntity
            {
                ActionType = ActionType.UserAction,
                Description = "Enter to Historical Module",
            });
        }

        /// <summary>
        /// Loads data based on the provided arguments.
        /// </summary>
        /// <param name="args">The arguments for loading data.</param>
        private async Task LoadData(LoadDataArgs args)
        {
            isLoading = true;

            var query = await HistoricalService.GetQueryable(new HistoricalFilterDTO
            {
                Skip = args.Skip ?? SKIP,
                Take = args.Top ?? TAKE,
            });

            count = query.TotalRows;
            logs = query.Rows.AsODataEnumerable();

            isLoading = false;
        }

        /// <summary>
        /// Shows the analysis result for the specified log entity.
        /// </summary>
        /// <param name="log">The log entity for which to show the analysis result.</param>
        private async Task ShowAnalysisResult(LogEntity log)
        {
            FileDataDTO details = JsonSerializer.Deserialize<FileDataDTO>(log.Details);

            var data = await HistoricalService.GetAnalysisResult(details.SHA256);

            await DialogService.OpenAsync<DialogAnalysisDetail>("Analysis Result", new()
            {
                { "AnalysisData", data },
            });
        }

        /// <summary>
        /// Exports data to Excel.
        /// </summary>
        private async Task ExportToExcel()
        {
            var stream = await HistoricalService.GetReport();
            var buffer = new byte[stream.Length];

            await stream.ReadAsync(buffer, 0, buffer.Length);
            var base64 = Convert.ToBase64String(buffer);
            var url = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}";

            NavigationManager.NavigateTo(url);
        }
    }
}
