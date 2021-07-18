using System.Collections.Generic;
using System.Linq;
using GodelTech.Data.Extensions;

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
        /// Deletes list of entities by their ids.
        /// </summary>
        /// <param name="ids">List of entities ids.</param>
        public virtual void Delete(IEnumerable<TKey> ids)
        {
            var entities = this.GetList(x => ids.Contains(x.Id));
            DbSet.RemoveRange(entities);
        }
    }
}