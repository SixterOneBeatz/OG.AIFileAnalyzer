using Microsoft.AspNetCore.Components;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    public partial class InvoiceFieldsTable
    {
        [Parameter]
        public IEnumerable<Tuple<string, string>> Fields { get; set; }
    }
}
