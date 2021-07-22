using System.Collections.ObjectModel;
using System.Linq;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Moq;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public partial class RepositoryTests
    {
        [Theory]
        [MemberData(nameof(QueryMemberData))]
        public void GetList_Success<TKey>(
            TKey defaultKey,
            QueryParameters<FakeEntity<TKey>, TKey> queryParameters,
            Collection<FakeEntity<TKey>> entities,
            IQueryable<FakeEntity<TKey>> queryableEntities,
            int filteredEntitiesCount)
        {
            // Arrange
            var expectedResult = queryableEntities.ToList();

            var repository = GetRepository<FakeEntity<TKey>, TKey>(entities);

            // Act
            var result = repository.GetList(queryParameters);

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(filteredEntitiesCount, queryableEntities.ToList().Count);
            Assert.Equal(expectedResult, result, new FakeEntityEqualityComparer<TKey>());
        }

        [Theory]
        [MemberData(nameof(QueryMemberData))]
        public void GetModelList_Success<TKey>(
            TKey defaultKey,
            QueryParameters<FakeEntity<TKey>, TKey> queryParameters,
            Collection<FakeEntity<TKey>> entities,
            IQueryable<FakeEntity<TKey>> queryableEntities,
            int filteredEntitiesCount)
        {
            // Arrange
            var expectedResult = queryableEntities
                .Select(
                    x => new FakeModel<TKey>
                    {
                        Id = x.Id,
                        Name = x.Name
                    }
                )
                .ToList();

            _mockDataMapper
                .Setup(
                    x => x.Map<FakeModel<TKey>>(
                        It.Is<IQueryable<FakeEntity<TKey>>>(
                            y => !y
                                .Except(queryableEntities)
                                .Any()
                        )
                    )
                )
                .Returns(
                    queryableEntities
                        .Select(
                            x => new FakeModel<TKey>
                            {
                                Id = x.Id,
                                Name = x.Name
                            }
                        )
                );

            var repository = GetRepository<FakeEntity<TKey>, TKey>(entities);

            // Act
            var result = repository.GetList<FakeModel<TKey>>(queryParameters);

            // Assert
            _mockDataMapper
                .Verify(
                    x => x.Map<FakeModel<TKey>>(
                        It.Is<IQueryable<FakeEntity<TKey>>>(
                            y => !y
                                .Except(queryableEntities)
                                .Any()
                        )
                    ),
                    Times.Once
                );

            Assert.NotNull(defaultKey);
            Assert.Equal(filteredEntitiesCount, queryableEntities.ToList().Count);
            Assert.Equal(expectedResult, result, new FakeModelEqualityComparer<TKey>());
        }
    }
}