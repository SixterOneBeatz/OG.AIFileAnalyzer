using Microsoft.EntityFrameworkCore;
using OG.AIFileAnalyzer.Common.Entities;
using OG.AIFileAnalyzer.Persistence.DataAccess.Contexts;
using System.Linq.Expressions;

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository
{
    public class BaseRepository<T>(AIFileAnalyzerDbContext context) : IBaseRepository<T> where T : BaseEntity
    {
        private readonly AIFileAnalyzerDbContext _context = context;

        public async Task AddEntity(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<(List<T> values, int total)> GetAsync(int skip, int take)
        {
            var query = _context.Set<T>();

            return (await query.Skip(skip).Take(take).ToListAsync(), query.Count());
        }

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }
    }
}
