using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public partial class RepositoryAsyncTests
    {
        [Theory]
        [MemberData(nameof(RepositoryTests.QueryMemberData), MemberType = typeof(RepositoryTests))]
        public async Task ExistsAsync_Success<TKey>(
            TKey defaultKey,
            QueryParameters<FakeEntity<TKey>, TKey> queryParameters,
            Collection<FakeEntity<TKey>> entities,
            IQueryable<FakeEntity<TKey>> queryableEntities,
            int filteredEntitiesCount)
        {
            // Arrange
            var expectedResult = queryableEntities.Any();

            var repository = GetRepository<FakeEntity<TKey>, TKey>(entities);

            // Act
            var result = await repository.ExistsAsync(queryParameters);

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(filteredEntitiesCount, queryableEntities.ToList().Count);
            Assert.Equal(expectedResult, result);
        }
    }
}