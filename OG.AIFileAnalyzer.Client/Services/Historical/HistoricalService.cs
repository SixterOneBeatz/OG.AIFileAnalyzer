using System.Net.Http;

namespace OG.AIFileAnalyzer.Client.Services.Historical
{
    public class HistoricalService(HttpClient httpClient) : BaseService(httpClient), IHistoricalService
    {
    }
}
