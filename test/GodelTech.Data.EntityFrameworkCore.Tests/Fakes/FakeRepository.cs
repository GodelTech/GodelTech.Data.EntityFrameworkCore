using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeRepository : Repository<FakeEntity, int>
    {
        public FakeRepository(DbContext dbContext, IDataMapper dataMapper)
            : base(dbContext, dataMapper)
        {

        }

        public IQueryable<FakeEntity> ProtectedQuery(QueryParameters<FakeEntity, int> queryParameters = null)
        {
            return Query(queryParameters);
        }

        public IQueryable<TModel> ProtectedQuery<TModel>(QueryParameters<FakeEntity, int> queryParameters = null)
        {
            return Query<TModel>(queryParameters);
        }

        public IQueryable<FakeEntity> ProtectedPagedResultQuery(QueryParameters<FakeEntity, int> queryParameters)
        {
            return PagedResultQuery(queryParameters);
        }

        public IQueryable<TModel> ProtectedPagedResultQuery<TModel>(QueryParameters<FakeEntity, int> queryParameters)
        {
            return PagedResultQuery<TModel>(queryParameters);
        }

        public IQueryable<FakeEntity> ProtectedCountQuery(QueryParameters<FakeEntity, int> queryParameters = null)
        {
            return CountQuery(queryParameters);
        }
    }
}
