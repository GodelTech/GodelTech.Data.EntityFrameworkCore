using System.Collections.ObjectModel;
using System.Linq;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public partial class RepositoryTests
    {
        [Theory]
        [MemberData(nameof(QueryMemberData))]
        public void Count_Success<TKey>(
            TKey defaultKey,
            QueryParameters<FakeEntity<TKey>, TKey> queryParameters,
            Collection<FakeEntity<TKey>> entities,
            IQueryable<FakeEntity<TKey>> queryableEntities,
            int filteredEntitiesCount)
        {
            // Arrange
            var query = entities.AsQueryable();

            if (queryParameters?.Filter?.Expression != null)
            {
                query = query.Where(queryParameters.Filter.Expression);
            }

            var expectedResult = query.Count();

            var repository = GetRepository<FakeEntity<TKey>, TKey>(entities);

            // Act
            var result = repository.Count(queryParameters);

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(filteredEntitiesCount, queryableEntities.ToList().Count);
            Assert.Equal(expectedResult, result);
        }
    }
}
