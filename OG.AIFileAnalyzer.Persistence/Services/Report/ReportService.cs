using OfficeOpenXml;
using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Persistence.Services.Report
{
    public class ReportService : IReportService
    {
        public MemoryStream ExportToExcel(List<LogEntity> logs)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Logs");
                worksheet.Cells.LoadFromCollection(logs, true);
                var stream = new MemoryStream(package.GetAsByteArray());
                return stream;
            }
        }
    }
}
