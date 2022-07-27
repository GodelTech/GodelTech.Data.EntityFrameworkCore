using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.Simple
{
    /// <summary>
    /// Repository for data layer without Unit of Work. Automatically saves changes.
    /// </summary>
    /// <typeparam name="TEntity">The type of the T entity.</typeparam>
    /// <typeparam name="TKey">The type of the T key.</typeparam>
    public partial class SimpleRepository<TEntity, TKey> : Repository<TEntity, TKey>, ISimpleRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleRepository{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dataMapper">The data mapper.</param>
        public SimpleRepository(DbContext dbContext, IDataMapper dataMapper)
            : base(dbContext, dataMapper)
        {

        }
    }
}
