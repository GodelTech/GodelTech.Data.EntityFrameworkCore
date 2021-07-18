using System.Collections.Generic;
using System.Threading.Tasks;

namespace GodelTech.Data.EntityFrameworkCore
{
    public partial class Repository<TEntity, TType>
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
        /// <returns><cref>TEntity</cref>.</returns>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var entityEntry = await DbSet.AddAsync(entity);

            return entityEntry.Entity;
        }

        /// <summary>
        /// Asynchronously inserts list of entities in the repository.
        /// </summary>
        /// <param name="entities">List of entities</param>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }
    }
}