using System.Collections.ObjectModel;
using System.Linq;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Simple
{
    public partial class SimpleRepositoryTests
    {
        [Theory]
        [MemberData(nameof(RepositoryTests.InsertMemberData), MemberType = typeof(RepositoryTests))]
        public void Insert_Success<TKey>(
            TKey defaultKey,
            FakeEntity<TKey> entity,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange
            var repository = GetRepository<TKey>();

            // Act
            var result = repository.Insert(entity);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextEntityResult = DbContext
                .Set<FakeEntity<TKey>>()
                .Single(x => x.Id.Equals(entity.Id));

            Assert.Equal(entity, result);
            Assert.Equal(dbContextEntityResult, result, new FakeEntityEqualityComparer<TKey>());

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            Assert.Equal(expectedEntities, dbContextResult, new FakeEntityEqualityComparer<TKey>());
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.InsertListMemberData), MemberType = typeof(RepositoryTests))]
        public void InsertList_Success<TKey>(
            TKey defaultKey,
            Collection<FakeEntity<TKey>> entities,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange
            var repository = GetRepository<TKey>();

            // Act
            repository.Insert(entities);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            Assert.Equal(expectedEntities, dbContextResult, new FakeEntityEqualityComparer<TKey>());
        }
    }
}
