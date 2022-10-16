using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Simple
{
    public partial class SimpleRepositoryAsyncTests
    {
        [Theory]
        [MemberData(nameof(RepositoryTests.UpdateMemberData), MemberType = typeof(RepositoryTests))]
        public async Task UpdateAsync_Success<TKey>(
            TKey defaultKey,
            FakeEntity<TKey> entity,
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
            var result = await repository.UpdateAsync(entity, cancellationToken: cancellationToken);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextEntityResult = DbContext
                .Set<FakeEntity<TKey>>()
                .Single(x => x.Id.Equals(entity.Id));

            Assert.Equal(entity, result);
            result.Should().BeEquivalentTo(dbContextEntityResult);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            dbContextResult.Should().BeEquivalentTo(expectedEntities);
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.UpdateMemberData), MemberType = typeof(RepositoryTests))]
        public async Task UpdateAsync_WithStartTrackProperties_EntityNotMarkedAsModified<TKey>(
            TKey defaultKey,
            FakeEntity<TKey> entity,
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
            var result = await repository.UpdateAsync(entity, true, cancellationToken);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextEntityResult = await DbContext
                .Set<FakeEntity<TKey>>()
                .SingleAsync(x => x.Id.Equals(entity.Id), cancellationToken);

            Assert.Equal(entity, result);
            result.Should().BeEquivalentTo(dbContextEntityResult);

            var dbContextResult = await DbContext
                .Set<FakeEntity<TKey>>()
                .ToListAsync(cancellationToken);

            dbContextResult.Should().BeEquivalentTo(expectedEntities);
        }
    }
}
