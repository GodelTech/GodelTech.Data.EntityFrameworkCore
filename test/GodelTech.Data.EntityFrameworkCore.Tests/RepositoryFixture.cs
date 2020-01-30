using System;
using System.Linq;
using AutoMapper;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public class RepositoryFixture : IDisposable
    {
        public RepositoryFixture()
        {
            // AutoMapper
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Entity<int>, FakeModel>());

            // data mapper
            DataMapper = new FakeDataMapper();

            // database
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<FakeDbContext>().UseInMemoryDatabase($"{nameof(RepositoryFixture)}{Guid.NewGuid():N}");

            UnitOfWork = new FakeUnitOfWork(
                dbContext => new FakeRepository(dbContext),
                dbContextOptionsBuilder.Options,
                "dbo"
            );

            UnitOfWork.FakeEntityRepository.Insert(new FakeEntity());
            UnitOfWork.FakeEntityRepository.Insert(new FakeEntity());
            UnitOfWork.FakeEntityRepository.Insert(new FakeEntity());
            UnitOfWork.FakeEntityRepository.Insert(new FakeEntity());
            UnitOfWork.FakeEntityRepository.Insert(new FakeEntity());
            UnitOfWork.Commit();

            ExistingFakeEntity = UnitOfWork.FakeEntityRepository.GetList().First();
            ExistingFakeModel = UnitOfWork.FakeEntityRepository.GetList<FakeModel>(DataMapper).First();
        }

        public FakeDataMapper DataMapper { get; }

        public FakeUnitOfWork UnitOfWork { get; }

        public FakeEntity ExistingFakeEntity { get; }

        public FakeModel ExistingFakeModel { get; }

        public void Dispose()
        {
            UnitOfWork.Dispose();
        }
    }
}
