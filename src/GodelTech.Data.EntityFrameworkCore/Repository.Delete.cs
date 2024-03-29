﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
            foreach (var entity in entities.Where(IsDetached))
            {
                DbSet.Attach(entity);
            }

            DbSet.RemoveRange(entities);
        }

        /// <inheritdoc />
        public virtual Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = new CancellationToken())
        {
            Delete(entity);

            return Task.CompletedTask;
        }

        /// <inheritdoc />
        public virtual Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = new CancellationToken())
        {
            Delete(entities);

            return Task.CompletedTask;
        }
    }
}
