using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using OG.AIFileAnalyzer.Client.Services.Analyzer;
using OG.AIFileAnalyzer.Client.Services.Historical;
using OG.AIFileAnalyzer.Common.Consts;
using OG.AIFileAnalyzer.Common.Entities;
using Radzen;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    /// <summary>
    /// Partial class representing the DragNDrop component.
    /// </summary>
    public partial class DragNDrop
    {
        /// <summary>
        /// Injected instance of the analyzer service.
        /// </summary>
        [Inject]
        private IAnalyzerService AnalyzerService { get; set; }

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
        /// Injected instance of the notification service.
        /// </summary>
        [Inject]
        private NotificationService NotificationService { get; set; }

        /// <summary>
        /// Represents the loading state of the component.
        /// </summary>
        bool IsLoading = false;

        /// <summary>
        /// Handles the change event when a file is selected.
        /// </summary>
        /// <param name="e">The event arguments containing information about the selected file.</param>
        async Task OnChange(InputFileChangeEventArgs e)
        {
            var file = e.File;
            try
            {
                IsLoading = true;

                if (IsValidFile(file))
                {
                    var buffer = new byte[file.Size];
                    await file.OpenReadStream().ReadAsync(buffer);

                    var base64String = Convert.ToBase64String(buffer);

                    await HistoricalService.Add(new LogEntity
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
                else
                {
                    NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Warning, Summary = "Warning", Detail = "Invalid File!" });
                }
            }
            catch (Exception)
            {
                NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = "Error", Detail = "Please contact the administrator" });
            }
            finally
            {
                IsLoading = false;
            }
        }

        /// <summary>
        /// Checks if the selected file is valid.
        /// </summary>
        /// <param name="file">The selected file.</param>
        /// <returns>True if the file is valid; otherwise, false.</returns>
        bool IsValidFile(IBrowserFile file)
        {
            bool result = true;

            var fileExtension = Path.GetExtension(file.Name).TrimStart('.').ToLower();

            result &= file != null;
            result &= Enum.TryParse(fileExtension, true, out AllowedExtension extension);
            result &= Enum.IsDefined(typeof(AllowedExtension), extension);

            return result;
        }
    }
}
