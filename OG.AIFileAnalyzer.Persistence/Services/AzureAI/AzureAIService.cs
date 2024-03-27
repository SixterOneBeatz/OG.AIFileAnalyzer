using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Azure.AI.FormRecognizer.Models;
using Azure.AI.TextAnalytics;
using Microsoft.Extensions.Configuration;
using OG.AIFileAnalyzer.Common.DTOs;

namespace OG.AIFileAnalyzer.Persistence.Services.AzureAI
{
    public class AzureAIService : IAzureAIService
    {
        private readonly DocumentAnalysisClient _documentAnalysisClient;
        private readonly FormRecognizerClient _formRecognizerClient;
        private readonly TextAnalyticsClient _textAnalyticsClient;
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

        public async Task<AnalysisResponseDTO> RunInvoiceAnalysis(MemoryStream document)
        {
            AnalyzeDocumentOperation operation = await _documentAnalysisClient.AnalyzeDocumentAsync(WaitUntil.Completed, "prebuilt-invoice", document);
            AnalyzeResult result = operation.Value;

            var docto = result.Documents.FirstOrDefault();

            var keyValuePairs = docto.Fields.Select(x => new { x.Key, Value = x.Value.Content }).ToDictionary(x => x.Key, y => y.Value);
            var docType = docto.DocumentType;

            return new()
            {
                Data = keyValuePairs,
                DocumentType = docType,
            };
        }

        public async Task<AnalysisResponseDTO> RunTextAnalysis(MemoryStream document)
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

            return new()
            {
                Data = data,
                DocumentType = "Text General"
            };
        }
    }
}
