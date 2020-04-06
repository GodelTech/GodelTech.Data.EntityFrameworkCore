using System;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeUnitOfWork : UnitOfWork
    {
        public FakeUnitOfWork(
            Func<DbContext, FakeRepository> fakeEntityRepository,
            DbContextOptions dbContextOptions,
            string schemaName)
            : base(new FakeDbContext(dbContextOptions, schemaName))
        {
            RegisterRepository(fakeEntityRepository(DbContext));
        }

        public FakeUnitOfWork()
            : base(null)
        {
            RegisterRepository(new FakeRepository(DbContext));
        }

        public FakeRepository FakeEntityRepository => (FakeRepository)GetRepository<FakeEntity, int>();

        public void DoNotDispose()
        {
            Dispose(false);
        }
    }
}
