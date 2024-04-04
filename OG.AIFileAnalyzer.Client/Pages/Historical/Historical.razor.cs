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
        /// Injected instance of the tooltip service.
        /// </summary>
        [Inject]
        private TooltipService TooltipService { get; set; }

        /// <summary>
        /// Injected instance of the notification service.
        /// </summary>
        [Inject]
        private NotificationService NotificationService { get; set; }

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
        private ODataEnumerable<LogEntity> Logs;

        /// <summary>
        /// Data grid component for displaying log entities.
        /// </summary>
        private RadzenDataGrid<LogEntity> LogsGrid;

        /// <summary>
        /// Indicates whether data is being loaded.
        /// </summary>
        private bool IsLoading;

        /// <summary>
        /// The total count of log entities.
        /// </summary>
        private int Count;

        /// <summary>
        /// Default tooltip options
        /// </summary>
        private readonly TooltipOptions TooltipOptions = new()
        {
            Position = TooltipPosition.Left, 
            Duration = null
        };

        /// <summary>
        /// Action types for dropdown
        /// </summary>
        private readonly IEnumerable<ActionType> ActionTypes = Enum.GetValues(typeof(ActionType)).Cast<ActionType>();

        private HistoricalFilterDTO Filter = new();

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
            IsLoading = true;
            
            Filter.Skip = args.Skip ?? SKIP;
            Filter.Take = args.Top ?? TAKE;
            
            var query = await HistoricalService.GetQueryable(Filter);

            Count = query.TotalRows;
            Logs = query.Rows.AsODataEnumerable();

            IsLoading = false;
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
            }, new()
            {
                Width = "50vw"
            });
        }

        /// <summary>
        /// Exports data to Excel.
        /// </summary>
        private async Task ExportToExcel()
        {
            if (IsValidFilter())
            {
                var stream = await HistoricalService.GetReport(Filter);
                var buffer = new byte[stream.Length];

                await stream.ReadAsync(buffer, 0, buffer.Length);
                var base64 = Convert.ToBase64String(buffer);
                var url = $"data:application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;base64,{base64}";

                NavigationManager.NavigateTo(url);
            }
        }

        /// <summary>
        /// Triggers search for data
        /// </summary>
        /// <returns></returns>
        private async Task Search()
        {
            if (IsValidFilter())
            {
                await LogsGrid.Reload();
            }
        }

        /// <summary>
        /// Clears the filter value
        /// </summary>
        private void Clean()
        {
            Filter = new();
        }

        /// <summary>
        /// Validates if value of filter is valid
        /// </summary>
        /// <returns></returns>
        private bool IsValidFilter()
        {
            bool isValid = (Filter.DateStart ?? DateTime.MinValue) <= (Filter.DateEnd ?? DateTime.MaxValue);
            if (!isValid)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Warning", Detail = "Invalid date range!" });
            }
            return isValid;
        }
    }
}
