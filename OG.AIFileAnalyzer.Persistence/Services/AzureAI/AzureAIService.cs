using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.AI.FormRecognizer.Models;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace OG.AIFileAnalyzer.Persistence.Services.AzureAI
{
    public class AzureAIService : IAzureAIService
    {
        private readonly DocumentAnalysisClient _documentAnalysisClient;
        private readonly FormRecognizerClient _formRecognizerClient;
        private readonly TextAnalyticsClient _textAnalyticsClient;

        // Metadata: Describes the purpose of the constructor and its parameters
        /// <summary>
        /// Initializes a new instance of the <see cref="AzureAIService"/> class.
        /// </summary>
        /// <param name="configuration">The configuration containing Azure AI service settings.</param>
        public AzureAIService(IConfiguration configuration)
        {
            string formRecognizerKey = configuration["AzureAI:formRecognizerKey"];
            string formRecognizerEndPoint = configuration["AzureAI:formRecognizerEndPoint"];
            string textAnalyzeKey = configuration["AzureAI:textAnalyzeKey"];
            string textAnalyzeEndPoint = configuration["AzureAI:textAnalyzeEndPoint"];

            _documentAnalysisClient = new DocumentAnalysisClient(new Uri(formRecognizerEndPoint), new AzureKeyCredential(formRecognizerKey));
            _formRecognizerClient = new FormRecognizerClient(new Uri(formRecognizerEndPoint), new AzureKeyCredential(formRecognizerKey));
            _textAnalyticsClient = new TextAnalyticsClient(new Uri(textAnalyzeEndPoint), new AzureKeyCredential(textAnalyzeKey));
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, string>> RunInvoiceAnalysis(MemoryStream document)
        {
            AnalyzeDocumentOperation operation = await _documentAnalysisClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", document);
            AnalyzeResult result = operation.Value;

            var documentFields = result.Documents.FirstOrDefault()?.Fields;

            var keyValuePairs = documentFields?
                .Select(x => new { x.Key, Value = x.Value.Content })
                .Where(y => y.Key != "Items").ToDictionary(x => x.Key, y => y.Value) ?? 
                new ();

            if (documentFields.TryGetValue("Items", out var invoiceItems) && invoiceItems.FieldType == DocumentFieldType.List)
            {
                var items = invoiceItems.Value.AsList()
                    .Where(x => x.FieldType == DocumentFieldType.Dictionary && x.Value.AsDictionary().Count > 0)
                    .Select(y => y.Value.AsDictionary()
                    .ToDictionary(k => k.Key, v => v.Value.Content));

                keyValuePairs.Add("Items", JsonSerializer.Serialize(items));
                keyValuePairs.Add("TotalItems", $"{items.Count()}");
            }

            return keyValuePairs;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, string>> RunTextAnalysis(MemoryStream document)
        {
            RecognizeContentOperation formOperation = await _formRecognizerClient.StartRecognizeContentAsync(document);

            // Wait for the operation to complete
            await formOperation.WaitForCompletionAsync();

            // Extracted text
            string extractedText = "";

            // Extract text and aggregate into a single string
            FormPageCollection formPages = formOperation.Value;
            foreach (FormPage page in formPages)
            {
                foreach (FormLine line in page.Lines)
                {
                    extractedText += line.Text + " ";
                }
            }

            var data = new Dictionary<string, string>();

            // Analyze sentiment using Text Analytics
            DocumentSentiment documentSentiment = await _textAnalyticsClient.AnalyzeSentimentAsync(extractedText);
            AbstractiveSummarizeOperation abstractiveSummarizeOperation = await _textAnalyticsClient.AbstractiveSummarizeAsync(WaitUntil.Completed, new List<string> { extractedText });

            data.Add("Sentiment", documentSentiment.Sentiment.ToString());

            int summaryNumber = 1;
            foreach (AbstractiveSummarizeResultCollection documentsInPage in abstractiveSummarizeOperation.GetValues())
            {
                foreach (AbstractiveSummarizeResult documentResult in documentsInPage)
                {
                    foreach (AbstractiveSummary summary in documentResult.Summaries)
                    {
                        data.Add($"Summary {summaryNumber}:", summary.Text.Replace("\n", " "));
                    }
                }
            }

            return data;
        }
    }
}
