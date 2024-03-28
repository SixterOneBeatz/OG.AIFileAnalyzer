using Microsoft.EntityFrameworkCore;
using OG.AIFileAnalyzer.Common.Entities;
using OG.AIFileAnalyzer.Persistence.DataAccess.Contexts;
using System.Linq.Expressions;

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository
{
    /// <summary>
    /// Generic repository for database operations.
    /// </summary>
    /// <typeparam name="T">Type of entity for repository.</typeparam>
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

        public async Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate = null, List<Expression<Func<T, object>>> includes = null)
        {

            IQueryable<T> query = _context.Set<T>();
            
            if (includes != null) query = includes.Aggregate(query, (current, include) => current.Include(include));

            if (predicate != null) query = query.Where(predicate);


            return await query.ToListAsync();
        }
    }
}
