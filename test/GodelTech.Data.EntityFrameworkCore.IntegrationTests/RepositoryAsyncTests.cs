using System;
using AutoMapper;
using AutoMapper.Configuration;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests
{
    public sealed partial class RepositoryAsyncTests : IDisposable
    {
        private readonly FakeUnitOfWork _unitOfWork;

        public RepositoryAsyncTests()
        {
            // AutoMapper
            var autoMapperConfig = new MapperConfiguration(
                new MapperConfigurationExpression()
            );

            var autoMapper = autoMapperConfig.CreateMapper();

            // data mapper
            var dataMapper = new FakeDataMapper(autoMapper);

            // database
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase($"{nameof(RepositoryTests)}{Guid.NewGuid():N}");

            _unitOfWork = new FakeUnitOfWork(
                dbContext => new Repository<FakeEntity<Guid>, Guid>(dbContext, dataMapper),
                dbContext => new Repository<FakeEntity<int>, int>(dbContext, dataMapper),
                dbContext => new Repository<FakeEntity<string>, string>(dbContext, dataMapper),
                dbContextOptionsBuilder.Options,
                "dbo"
            );
        }

        public DbContext DbContext => _unitOfWork.ExposedDbContext;

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }
    }
}