﻿using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Simple
{
    public partial class SimpleRepositoryAsyncTests
    {
        [Theory]
        [MemberData(nameof(RepositoryTests.InsertMemberData), MemberType = typeof(RepositoryTests))]
        public async Task InsertAsync_Success<TKey>(
            TKey defaultKey,
            FakeEntity<TKey> entity,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            var repository = GetRepository<TKey>();

            // Act
            var result = await repository.InsertAsync(entity, cancellationToken);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextEntityResult = await DbContext
                .Set<FakeEntity<TKey>>()
                .SingleAsync(
                    x => x.Id.Equals(entity.Id),
                    cancellationToken
                );

            Assert.Equal(entity, result);
            Assert.Equal(dbContextEntityResult, result, new FakeEntityEqualityComparer<TKey>());

            var dbContextResult = await DbContext
                .Set<FakeEntity<TKey>>()
                .ToListAsync(cancellationToken);

            Assert.Equal(expectedEntities, dbContextResult, new FakeEntityEqualityComparer<TKey>());
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.InsertListMemberData), MemberType = typeof(RepositoryTests))]
        public async Task InsertListAsync_Success<TKey>(
            TKey defaultKey,
            Collection<FakeEntity<TKey>> entities,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            var repository = GetRepository<TKey>();

            // Act
            await repository.InsertAsync(entities, cancellationToken);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = await DbContext
                .Set<FakeEntity<TKey>>()
                .ToListAsync(cancellationToken);

            Assert.Equal(expectedEntities, dbContextResult, new FakeEntityEqualityComparer<TKey>());
        }
    }
}