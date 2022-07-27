using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GodelTech.Data.EntityFrameworkCore.Simple
{
    /// <summary>
    /// Interface of repository for data layer without Unit of Work. Automatically saves changes.
    /// </summary>
    /// <typeparam name="TEntity">The type of the T entity.</typeparam>
    /// <typeparam name="TKey">The type of the T key.</typeparam>
    public interface ISimpleRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Asynchronously updates entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="startTrackProperties">if set to <c>true</c> marks entity as modified.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns><cref>TEntity</cref>.</returns>
        Task<TEntity> UpdateAsync(TEntity entity, bool startTrackProperties = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously deletes range of entities.
        /// </summary>
        /// <param name="entities">List of entities</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}
