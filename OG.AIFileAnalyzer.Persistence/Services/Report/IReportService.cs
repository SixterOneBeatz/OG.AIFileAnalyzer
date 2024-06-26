﻿using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Persistence.Services.Report
{
    /// <summary>
    /// Service for exporting log data to Excel format.
    /// </summary>
    public interface IReportService
    {
        /// <summary>
        /// Exports a list of log entities to an Excel MemoryStream.
        /// </summary>
        /// <param name="logs">List of log entities to be exported.</param>
        /// <returns>MemoryStream containing Excel data.</returns>
        MemoryStream ExportToExcel(List<LogEntity> logs);
    }
}
