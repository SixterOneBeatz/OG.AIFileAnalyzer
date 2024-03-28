using OfficeOpenXml;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Persistence.Services.Report
{
    public class ReportService : IReportService
    {
        /// <inheritdoc/>
        public MemoryStream ExportToExcel(List<LogEntity> logs)
        {
            // Set license context for EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                // Create worksheet and load data
                var worksheet = package.Workbook.Worksheets.Add("Logs");
                worksheet.Cells.LoadFromCollection(logs, true);

                // Convert ExcelPackage to MemoryStream and return
                var stream = new MemoryStream(package.GetAsByteArray());
                return stream;
            }
        }
    }
}
