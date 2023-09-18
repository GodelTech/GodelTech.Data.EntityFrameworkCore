using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AutoMapper;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests
{
    public sealed class UnitOfWorkTests : IDisposable
    {
        private readonly FakeUnitOfWork _unitOfWork;

        public UnitOfWorkTests()
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
        }

        public DbContext DbContext => _unitOfWork.ExposedDbContext;

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        public static IEnumerable<object[]> TypesMemberData =>
            new Collection<object[]>
            {
                // Guid
                new object[]
                {
                    default(Guid)
                },
                // int
                new object[]
                {
                    default(int)
                },
                // string
                new object[]
                {
                    string.Empty
                }
            };

        [Theory]
        [MemberData(nameof(TypesMemberData))]
        public void ClearChangeTracker_Success<TKey>(TKey defaultKey)
        {
            // Arrange
            var entity = new FakeEntity<TKey>();

            _unitOfWork.ExposedDbContext.Attach(entity);

            // Act
            _unitOfWork.ClearChangeTracker();

            // Assert
            Assert.NotNull(defaultKey);

            Assert.Equal(
                EntityState.Detached,
                _unitOfWork.ExposedDbContext.Entry(entity).State
            );
        }
    }
}
