//using System;
//using Microsoft.EntityFrameworkCore;

//namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes
//{
//    public class FakeUnitOfWork : UnitOfWork
//    {
//        public FakeUnitOfWork(DbContext dbContext)
//            : base(dbContext)
//        {

//        }

//        //public FakeUnitOfWork(
//        //    Func<DbContext, FakeRepository> fakeEntityRepository,
//        //    DbContextOptions dbContextOptions,
//        //    string schemaName)
//        //    : base(new FakeDbContext(dbContextOptions, schemaName))
//        //{
//        //    RegisterRepository(fakeEntityRepository(DbContext));
//        //}

//        //public FakeUnitOfWork()
//        //    : base(null)
//        //{
//        //    RegisterRepository(new FakeRepository(DbContext, new FakeDataMapper()));
//        //}

//        //public FakeRepository FakeEntityRepository => (FakeRepository) GetRepository<FakeEntity, int>();

//        //public void DoNotDispose()
//        //{
//        //    Dispose(false);
//        //}
//    }
//}