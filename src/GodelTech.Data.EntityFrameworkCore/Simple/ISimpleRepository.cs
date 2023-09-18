namespace GodelTech.Data.EntityFrameworkCore.Simple
{
    /// <summary>
    /// Interface of repository for data layer without Unit of Work. Automatically saves changes.
    /// </summary>
    /// <typeparam name="TEntity">The type of the T entity.</typeparam>
    /// <typeparam name="TKey">The type of the T key.</typeparam>
    public interface ISimpleRepository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {

    }
}
