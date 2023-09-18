using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes
{
    public class FakeUnitOfWork : UnitOfWork<FakeDbContext>
    {
        public FakeUnitOfWork(
            Func<DbContext, Repository<FakeEntity<Guid>, Guid>> fakeGuidEntityRepository,
            Func<DbContext, Repository<FakeEntity<int>, int>> fakeIntEntityRepository,
            Func<DbContext, Repository<FakeEntity<string>, string>> fakeStringEntityRepository,
            IDesignTimeDbContextFactory<FakeDbContext> dbContextFactory)
            : base(dbContextFactory)
        {
            if (fakeGuidEntityRepository == null) throw new ArgumentNullException(nameof(fakeGuidEntityRepository));
            if (fakeIntEntityRepository == null) throw new ArgumentNullException(nameof(fakeIntEntityRepository));
            if (fakeStringEntityRepository == null) throw new ArgumentNullException(nameof(fakeStringEntityRepository));

            RegisterRepository(fakeGuidEntityRepository(DbContext));
            RegisterRepository(fakeIntEntityRepository(DbContext));
            RegisterRepository(fakeStringEntityRepository(DbContext));
        }

        public IRepository<FakeEntity<Guid>, Guid> FakeGuidEntityRepository => GetRepository<FakeEntity<Guid>, Guid>();

        public IRepository<FakeEntity<int>, int> FakeIntEntityRepository => GetRepository<FakeEntity<int>, int>();

        public IRepository<FakeEntity<string>, string> FakeStringEntityRepository => GetRepository<FakeEntity<string>, string>();

        public DbContext ExposedDbContext => DbContext;

        public IRepository<FakeEntity<TKey>, TKey> GetFakeTypeEntityRepository<TKey>() => GetRepository<FakeEntity<TKey>, TKey>();
    }
}
