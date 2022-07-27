using System;
using System.Collections.Generic;
using AutoMapper;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using GodelTech.Data.EntityFrameworkCore.Simple;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Simple
{
    public sealed partial class SimpleRepositoryTests : IDisposable
    {
        private readonly IDictionary<Type, object> _repositories = new Dictionary<Type, object>();

        public SimpleRepositoryTests()
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
                .UseInMemoryDatabase($"{nameof(SimpleRepositoryTests)}{Guid.NewGuid():N}");

            DbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");

            _repositories[typeof(FakeEntity<Guid>)] = new SimpleRepository<FakeEntity<Guid>, Guid>(DbContext, dataMapper);
            _repositories[typeof(FakeEntity<int>)] = new SimpleRepository<FakeEntity<int>, int>(DbContext, dataMapper);
            _repositories[typeof(FakeEntity<string>)] = new SimpleRepository<FakeEntity<string>, string>(DbContext, dataMapper);
        }

        public DbContext DbContext { get; }

        public void Dispose()
        {
            DbContext.Dispose();
        }

        public ISimpleRepository<FakeEntity<TKey>, TKey> GetRepository<TKey>()
        {
            return (ISimpleRepository<FakeEntity<TKey>, TKey>) _repositories[typeof(FakeEntity<TKey>)];
        }
    }
}
