using OG.AIFileAnalyzer.Common.Entities;
using System.Linq.Expressions;

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository
{
    public interface IBaseRepository<T> where T : BaseEntity
    {
        Task<List<T>> GetAllAsync();
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);
        Task<(List<T> values, int total)> GetAsync(int skip, int take);
        Task AddEntity(T entity);
    }
}
