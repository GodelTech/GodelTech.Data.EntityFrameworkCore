using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GodelTech.Data.EntityFrameworkCore.Simple
{
    public partial class SimpleRepository<TEntity, TKey>
    {
        /// <inheritdoc />
        public override void Delete(TEntity entity)
        {
            base.Delete(entity);

            DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public override void Delete(IEnumerable<TEntity> entities)
        {
            base.Delete(entities);

            DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            base.Delete(entity);

            await DbContext.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            base.Delete(entities);

            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
