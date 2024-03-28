using OG.AIFileAnalyzer.Common.Entities;
using OG.AIFileAnalyzer.Persistence.DataAccess.Contexts;
using OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository;
using System.Collections;

namespace OG.AIFileAnalyzer.Persistence.DataAccess.UnitOfWork
{
    /// <summary>
    /// Implementation of the unit of work pattern for coordinating repository access and database transactions.
    /// </summary>
    /// <remarks>
    /// Constructs an instance of UnitOfWork with the specified database context.
    /// </remarks>
    /// <param name="context">The database context.</param>
    public class UnitOfWork(AIFileAnalyzerDbContext context) : IUnitOfWork
    {
        private readonly AIFileAnalyzerDbContext _context = context;

        private Hashtable _repositories;

        /// <inheritdoc/>
        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public void Dispose() => _context.Dispose();

        /// <inheritdoc/>
        public IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            _repositories ??= new Hashtable();

            var type = typeof(TEntity).Name;

            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);
                _repositories.Add(type, repositoryInstance);
            }

            return (IBaseRepository<TEntity>)_repositories[type];
        }

        /// <summary>
        /// Finalizes an instance of the UnitOfWork class.
        /// </summary>
        ~UnitOfWork() => Dispose();
    }
}
