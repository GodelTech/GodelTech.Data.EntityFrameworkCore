using System;
using AutoMapper;
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
            var dbContextOptions = new DbContextOptionsBuilder<FakeDbContext>()
                .UseSqlite("DataSource=:memory:")
                .Options;

            var dbContextFactory = new FakeDbContextFactory(dbContextOptions);

            _unitOfWork = new FakeUnitOfWork(
                dbContext => new Repository<FakeEntity<Guid>, Guid>(dbContext, dataMapper),
                dbContext => new Repository<FakeEntity<int>, int>(dbContext, dataMapper),
                dbContext => new Repository<FakeEntity<string>, string>(dbContext, dataMapper),
                dbContextFactory
            );

            DbContext.Database.OpenConnection();
            DbContext.Database.EnsureCreated();
        }

        public DbContext DbContext => _unitOfWork.ExposedDbContext;

        public void Dispose()
        {
            DbContext.Database.CloseConnection();
            _unitOfWork.Dispose();
        }
    }
}
