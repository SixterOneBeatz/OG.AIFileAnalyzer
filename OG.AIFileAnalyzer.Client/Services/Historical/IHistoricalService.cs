using OG.AIFileAnalyzer.Common.Entities;

namespace OG.AIFileAnalyzer.Client.Services.Historical
{
    public interface IHistoricalService
    {
        Task<List<LogEntity>> GetAll();
        Task Add(LogEntity log);
    }
}
