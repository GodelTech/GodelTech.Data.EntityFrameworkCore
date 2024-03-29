﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public async Task GetPagedListAsync_WhenQueryParametersIsNull_ThrowsArgumentNullException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            var repository = GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => repository.GetPagedListAsync(null, cancellationToken)
            );
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public async Task GetPagedListAsync_WhenQueryParametersPageIsNull_ThrowsArgumentException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            var repository = GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => repository.GetPagedListAsync(
                    new QueryParameters<FakeEntity<TKey>, TKey>
                    {
                        Page = null
                    },
                    cancellationToken
                )
            );
            Assert.Equal("Page can't be null. (Parameter 'queryParameters')", exception.Message);
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public async Task GetPagedListAsync_WhenQueryParametersPageIsNotValid_ThrowsArgumentException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            var repository = GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => repository.GetPagedListAsync(
                    new QueryParameters<FakeEntity<TKey>, TKey>
                    {
                        Page = new PageRule
                        {
                            Index = -1,
                            Size = -1
                        }
                    },
                    cancellationToken
                )
            );
            Assert.Equal("Page is not valid. (Parameter 'queryParameters')", exception.Message);
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.PagedResultQueryMemberData), MemberType = typeof(RepositoryTests))]
        public async Task GetPagedListAsync_Success<TKey>(
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
            var result = await repository.GetPagedListAsync(queryParameters, cancellationToken);

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(filteredEntitiesCount, queryableEntities.ToList().Count);
            Assert.Equal(queryParameters?.Page.Index, result.PageIndex);
            Assert.Equal(queryParameters?.Page.Size, result.PageSize);
            result.Items.Should().BeEquivalentTo(expectedResult);
            Assert.Equal(
                (queryParameters?.Page.Size * queryParameters?.Page.Index) + filteredEntitiesCount,
                result.TotalCount
            );
        }

        [Theory]
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public async Task GetModelPagedListAsync_WhenQueryParametersIsNull_ThrowsArgumentNullException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            var repository = GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => repository.GetPagedListAsync<FakeModel<TKey>>(null, cancellationToken)
            );
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public async Task GetModelPagedListAsync_WhenQueryParametersPageIsNull_ThrowsArgumentException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            var repository = GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => repository.GetPagedListAsync<FakeModel<TKey>>(
                    new QueryParameters<FakeEntity<TKey>, TKey>
                    {
                        Page = null
                    },
                    cancellationToken
                )
            );
            Assert.Equal("Page can't be null. (Parameter 'queryParameters')", exception.Message);
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public async Task GetModelPagedListAsync_WhenQueryParametersPageIsNotValid_ThrowsArgumentException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            var repository = GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = await Assert.ThrowsAsync<ArgumentException>(
                () => repository.GetPagedListAsync<FakeModel<TKey>>(
                    new QueryParameters<FakeEntity<TKey>, TKey>
                    {
                        Page = new PageRule
                        {
                            Index = -1,
                            Size = -1
                        }
                    },
                    cancellationToken
                )
            );
            Assert.Equal("Page is not valid. (Parameter 'queryParameters')", exception.Message);
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.PagedResultQueryMemberData), MemberType = typeof(RepositoryTests))]
        public async Task GetModelPagedListAsync_Success<TKey>(
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
            var result = await repository.GetPagedListAsync<FakeModel<TKey>>(queryParameters, cancellationToken);

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
            Assert.Equal(queryParameters?.Page.Index, result.PageIndex);
            Assert.Equal(queryParameters?.Page.Size, result.PageSize);
            result.Items.Should().BeEquivalentTo(expectedResult);
            Assert.Equal(
                (queryParameters?.Page.Size * queryParameters?.Page.Index) + filteredEntitiesCount,
                result.TotalCount
            );
        }
    }
}
