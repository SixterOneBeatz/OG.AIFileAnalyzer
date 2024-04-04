using Microsoft.AspNetCore.Components;
using OG.AIFileAnalyzer.Common.DTOs;
using System.Text.Json;

namespace OG.AIFileAnalyzer.Client.Pages.Analyzer
{
    public partial class InvoiceItemsTable
    {
        [Parameter]
        public KeyValuePair<string, string> RawItems { get; set; }

        private List<ItemDTO> Items = new();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();

            Items = JsonSerializer.Deserialize<List<ItemDTO>>(RawItems.Value);
        }
    }
}
