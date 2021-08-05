using System.Collections.Generic;
using System.Linq;

namespace GodelTech.Data.EntityFrameworkCore
{
    public partial class Repository<TEntity, TKey>
    {
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Delete(TEntity entity)
        {
            if (IsDetached(entity))
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }

        /// <summary>
        /// Deletes list of entities.
        /// </summary>
        /// <param name="entities">List of entities</param>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            Delete(entities.ToList());
        }

        private void Delete(IList<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                if (IsDetached(entity))
                {
                    DbSet.Attach(entity);
                }
            }

            DbSet.RemoveRange(entities);
        }
    }
}