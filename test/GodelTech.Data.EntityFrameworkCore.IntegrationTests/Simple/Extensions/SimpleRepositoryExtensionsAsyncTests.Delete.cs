using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Simple.Extensions
{
    public partial class SimpleRepositoryExtensionsAsyncTests
    {
        [Theory]
        [MemberData(nameof(RepositoryTests.DeleteMemberData), MemberType = typeof(RepositoryTests))]
        public async Task DeleteAsync_Success<TKey>(
            TKey defaultKey,
            FakeEntity<TKey> entity,
            Collection<FakeEntity<TKey>> existingEntities,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            var cancellationToken = new CancellationToken();

            await DbContext
                .Set<FakeEntity<TKey>>()
                .AddRangeAsync(existingEntities, cancellationToken);

            await DbContext.SaveChangesAsync(cancellationToken);
            DbContext.ChangeTracker.Clear();

            var repository = GetRepository<TKey>();

            // Act
            await repository.DeleteAsync(entity.Id, cancellationToken);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = await DbContext
                .Set<FakeEntity<TKey>>()
                .ToListAsync(cancellationToken);

            dbContextResult.Should().BeEquivalentTo(expectedEntities);
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.DeleteListMemberData), MemberType = typeof(RepositoryTests))]
        public async Task DeleteListAsync_Success<TKey>(
            TKey defaultKey,
            Collection<FakeEntity<TKey>> entities,
            Collection<FakeEntity<TKey>> existingEntities,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            await DbContext
                .Set<FakeEntity<TKey>>()
                .AddRangeAsync(existingEntities, cancellationToken);

            await DbContext.SaveChangesAsync(cancellationToken);
            DbContext.ChangeTracker.Clear();

            var repository = GetRepository<TKey>();

            // Act
            await repository.DeleteAsync(entities.Select(x => x.Id), cancellationToken);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = await DbContext
                .Set<FakeEntity<TKey>>()
                .ToListAsync(cancellationToken);

            dbContextResult.Should().BeEquivalentTo(expectedEntities);
        }
    }
}
