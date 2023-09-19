using System.Threading;
using System.Threading.Tasks;

namespace GodelTech.Data.EntityFrameworkCore
{
    public partial class Repository<TEntity, TKey>
    {
        /// <summary>
        /// Updates entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="startTrackProperties">if set to <c>true</c> marks entity as modified.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Update(TEntity entity, bool startTrackProperties = false)
        {
            DbSet.Attach(entity);

            if (!startTrackProperties)
            {
                MarkAsModified(entity);
            }

            return entity;
        }

        /// <inheritdoc />
        public virtual Task<TEntity> UpdateAsync(TEntity entity, bool startTrackProperties = false,
            CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(
                Update(entity, startTrackProperties)
            );
        }
    }
}
