using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GodelTech.Data.EntityFrameworkCore
{
    public partial class Repository<TEntity, TKey>
    {
        /// <summary>
        /// Inserts entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Insert(TEntity entity)
        {
            return DbSet.Add(entity).Entity;
        }

        /// <summary>
        /// Inserts list of entities in the repository.
        /// </summary>
        /// <param name="entities">List of entities</param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        /// <summary>
        /// Asynchronously inserts entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns><cref>TEntity</cref>.</returns>
        public virtual async Task<TEntity> InsertAsync(
            TEntity entity,
            CancellationToken cancellationToken = default)
        {
            var entityEntry = await DbSet.AddAsync(entity, cancellationToken);

            return entityEntry.Entity;
        }

        /// <summary>
        /// Asynchronously inserts list of entities in the repository.
        /// </summary>
        /// <param name="entities">List of entities</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        public virtual async Task InsertAsync(
            IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            await DbSet.AddRangeAsync(entities, cancellationToken);
        }
    }
}
