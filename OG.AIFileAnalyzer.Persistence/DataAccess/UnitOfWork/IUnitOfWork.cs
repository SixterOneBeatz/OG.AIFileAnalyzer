using OG.AIFileAnalyzer.Common.Entities;
using OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository;

namespace OG.AIFileAnalyzer.Persistence.DataAccess.UnitOfWork
{
    /// <summary>
    /// Interface for a unit of work pattern, providing access to repositories and save changes functionality.
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Retrieves the repository for the specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of entity for which to retrieve the repository.</typeparam>
        /// <returns>The repository for the specified entity type.</returns>
        IBaseRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity;

        /// <summary>
        /// Saves changes asynchronously to the underlying data store.
        /// </summary>
        /// <returns>A task representing the asynchronous save operation. The task result contains the number of state entries written to the data store.</returns>
        Task<int> Complete();
    }
}
