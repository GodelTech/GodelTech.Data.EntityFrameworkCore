using System;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes
{
    public class FakeUnitOfWork : UnitOfWork
    {
        public FakeUnitOfWork(
            Func<DbContext, Repository<FakeEntity<Guid>, Guid>> fakeGuidEntityRepository,
            Func<DbContext, Repository<FakeEntity<int>, int>> fakeIntEntityRepository,
            Func<DbContext, Repository<FakeEntity<string>, string>> fakeStringEntityRepository,
            DbContextOptions dbContextOptions,
            string schemaName)
#pragma warning disable CA2000 // Dispose objects before losing scope
            : base(new FakeDbContext(dbContextOptions, schemaName))
#pragma warning restore CA2000 // Dispose objects before losing scope
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