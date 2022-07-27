using System.Threading;
using System.Threading.Tasks;

namespace GodelTech.Data.EntityFrameworkCore.Simple
{
    public partial class SimpleRepository<TEntity, TKey>
    {
        /// <inheritdoc />
        public override TEntity Update(TEntity entity, bool startTrackProperties = false)
        {
            var result = base.Update(entity, startTrackProperties);

            DbContext.SaveChanges();

            return result;
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> UpdateAsync(TEntity entity, bool startTrackProperties = false, CancellationToken cancellationToken = default)
        {
            var result = base.Update(entity, startTrackProperties);

            await DbContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }
}
