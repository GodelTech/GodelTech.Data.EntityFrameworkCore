﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using MockQueryable.Moq;
using Moq;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public partial class RepositoryAsyncTests
    {
        [Theory]
        [MemberData(nameof(RepositoryTests.QueryMemberData), MemberType = typeof(RepositoryTests))]
        public async Task GetListAsync_Success<TKey>(
            TKey defaultKey,
            QueryParameters<FakeEntity<TKey>, TKey> queryParameters,
            Collection<FakeEntity<TKey>> entities,
            IQueryable<FakeEntity<TKey>> queryableEntities,
            int filteredEntitiesCount)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            var expectedResult = queryableEntities.ToList();

            var repository = GetRepository<FakeEntity<TKey>, TKey>(entities);

            // Act
            var result = await repository.GetListAsync(queryParameters, cancellationToken);

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(filteredEntitiesCount, queryableEntities.ToList().Count);
            result.Should().BeEquivalentTo(expectedResult);
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.QueryMemberData), MemberType = typeof(RepositoryTests))]
        public async Task GetModelListAsync_Success<TKey>(
            TKey defaultKey,
            QueryParameters<FakeEntity<TKey>, TKey> queryParameters,
            Collection<FakeEntity<TKey>> entities,
            IQueryable<FakeEntity<TKey>> queryableEntities,
            int filteredEntitiesCount)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

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
                        .BuildMock()
                );

            var repository = GetRepository<FakeEntity<TKey>, TKey>(entities);

            // Act
            var result = await repository.GetListAsync<FakeModel<TKey>>(queryParameters, cancellationToken);

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
            result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
