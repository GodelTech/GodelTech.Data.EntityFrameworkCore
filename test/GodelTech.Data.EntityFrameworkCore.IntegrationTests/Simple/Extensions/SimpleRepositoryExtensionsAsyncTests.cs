using System;
using System.Collections.Generic;
using AutoMapper;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using GodelTech.Data.EntityFrameworkCore.Simple;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Simple.Extensions
{
    public sealed partial class SimpleRepositoryExtensionsAsyncTests : IDisposable
    {
        private readonly IDictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public SimpleRepositoryExtensionsAsyncTests()
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

            DbContext = new FakeDbContext(dbContextOptions, "dbo");

            DbContext.Database.OpenConnection();
            DbContext.Database.EnsureCreated();

            _repositories[typeof(FakeEntity<Guid>)] = new SimpleRepository<FakeEntity<Guid>, Guid>(DbContext, dataMapper);
            _repositories[typeof(FakeEntity<int>)] = new SimpleRepository<FakeEntity<int>, int>(DbContext, dataMapper);
            _repositories[typeof(FakeEntity<string>)] = new SimpleRepository<FakeEntity<string>, string>(DbContext, dataMapper);
        }

        public DbContext DbContext { get; }

        public void Dispose()
        {
            DbContext.Database.CloseConnection();
            DbContext.Dispose();
        }

        public ISimpleRepository<FakeEntity<TKey>, TKey> GetRepository<TKey>()
        {
            return (ISimpleRepository<FakeEntity<TKey>, TKey>) _repositories[typeof(FakeEntity<TKey>)];
        }
    }
}
