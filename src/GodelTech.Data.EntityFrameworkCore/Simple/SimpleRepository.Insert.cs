using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace GodelTech.Data.EntityFrameworkCore.Simple
{
    public partial class SimpleRepository<TEntity, TKey>
    {
        /// <inheritdoc />
        public override TEntity Insert(TEntity entity)
        {
            var result = base.Insert(entity);

            DbContext.SaveChanges();

            return result;
        }

        /// <inheritdoc />
        public override void Insert(IEnumerable<TEntity> entities)
        {
            base.Insert(entities);

            DbContext.SaveChanges();
        }

        /// <inheritdoc />
        public override async Task<TEntity> InsertAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            var result = await base.InsertAsync(entity, cancellationToken);

            await DbContext.SaveChangesAsync(cancellationToken);

            return result;
        }

        /// <inheritdoc />
        public override async Task InsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await base.InsertAsync(entities, cancellationToken);

            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
