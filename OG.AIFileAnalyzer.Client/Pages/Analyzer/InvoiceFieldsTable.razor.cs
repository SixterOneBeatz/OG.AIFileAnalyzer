using Microsoft.AspNetCore.Components;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    /// <summary>
    /// Partial class representing the InvoiceFieldsTable component.
    /// </summary>
    public partial class InvoiceFieldsTable
    {
        /// <summary>
        /// Gets or sets the fields parameter.
        /// </summary>
        [Parameter]
        public Dictionary<string, string> Fields { get; set; }
    }
}
