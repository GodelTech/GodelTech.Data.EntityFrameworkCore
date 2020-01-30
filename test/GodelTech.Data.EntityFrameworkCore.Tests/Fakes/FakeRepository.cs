using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    [ExcludeFromCodeCoverage]
    public class FakeRepository : Repository<FakeEntity, int>
    {
        public FakeRepository(DbContext dbContext)
            : base(dbContext)
        {

        }

        public DbSet<FakeEntity> ProtectedDbSet => DbSet;

        public IQueryable<FakeEntity> ProtectedQuery(QueryParameters<FakeEntity, int> queryParameters = null)
        {
            return Query(queryParameters);
        }

        public IQueryable<TModel> ProtectedQuery<TModel>(IDataMapper dataMapper, QueryParameters<FakeEntity, int> queryParameters = null)
        {
            return Query<TModel>(dataMapper, queryParameters);
        }

        public IQueryable<FakeEntity> ProtectedPagedResultQuery(QueryParameters<FakeEntity, int> queryParameters)
        {
            return PagedResultQuery(queryParameters);
        }

        public IQueryable<TModel> ProtectedPagedResultQuery<TModel>(IDataMapper dataMapper, QueryParameters<FakeEntity, int> queryParameters)
        {
            return PagedResultQuery<TModel>(dataMapper, queryParameters);
        }

        public IQueryable<FakeEntity> ProtectedCountQuery(QueryParameters<FakeEntity, int> queryParameters = null)
        {
            return CountQuery(queryParameters);
        }
    }
}
