using OG.AIFileAnalyzer.Common.Entities;
using System.Linq.Expressions;

namespace OG.AIFileAnalyzer.Persistence.DataAccess.Repositories.BaseRepository
{
    /// <summary>
    /// Interface for generic repository operations.
    /// </summary>
    /// <typeparam name="T">Type of entity for repository.</typeparam>
    public interface IBaseRepository<T> where T : BaseEntity
    {
        /// <summary>
        /// Retrieves all entities asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation that returns a list of entities.</returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Retrieves entities that match the given predicate asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of matching entities.</returns>
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Retrieves entities that match the given predicate with optional includes asynchronously.
        /// </summary>
        /// <param name="predicate">The predicate to filter entities.</param>
        /// <param name="includes">The optional navigation properties to include.</param>
        /// <returns>A task representing the asynchronous operation that returns a list of matching entities.</returns>
        Task<List<T>> GetAsync(Expression<Func<T, bool>> predicate = null, List<Expression<Func<T, object>>> includes = null);

        /// <summary>
        /// Retrieves a subset of entities asynchronously.
        /// </summary>
        /// <param name="skip">The number of entities to skip.</param>
        /// <param name="take">The number of entities to take.</param>
        /// <returns>A task representing the asynchronous operation that returns a tuple containing a list of entities and the total count.</returns>
        Task<(List<T> values, int total)> GetAsync(int skip, int take);

        /// <summary>
        /// Adds an entity asynchronously.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddEntity(T entity);
    }
}