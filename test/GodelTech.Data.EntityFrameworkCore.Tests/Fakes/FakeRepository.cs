using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeRepository<TEntity, TKey> : Repository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public FakeRepository(DbContext dbContext, IDataMapper dataMapper)
            : base(dbContext, dataMapper)
        {

        }

        public IQueryable<TEntity> ExposedQuery(QueryParameters<TEntity, TKey> queryParameters = null)
        {
            return Query(queryParameters);
        }

        public IQueryable<TModel> ExposedQuery<TModel>(QueryParameters<TEntity, TKey> queryParameters = null)
        {
            return Query<TModel>(queryParameters);
        }

        public IQueryable<TEntity> ExposedPagedResultQuery(QueryParameters<TEntity, TKey> queryParameters)
        {
            return PagedResultQuery(queryParameters);
        }

        public IQueryable<TModel> ExposedPagedResultQuery<TModel>(QueryParameters<TEntity, TKey> queryParameters)
        {
            return PagedResultQuery<TModel>(queryParameters);
        }

        public IQueryable<TEntity> ExposedCountQuery(QueryParameters<TEntity, TKey> queryParameters = null)
        {
            return CountQuery(queryParameters);
        }

        // not possible to mock EntityEntry<TEntity>, so no usage in unit tests
        public void ExposedMarkAsModified(TEntity entity)
        {
            MarkAsModified(entity);
        }

        // not possible to mock EntityEntry<TEntity>, so no usage in unit tests
        public bool ExposedIsDetached(TEntity entity)
        {
            return IsDetached(entity);
        }
    }
}
