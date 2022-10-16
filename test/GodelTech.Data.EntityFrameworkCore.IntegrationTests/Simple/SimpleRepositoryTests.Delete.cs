using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Simple
{
    public partial class SimpleRepositoryTests
    {
        [Theory]
        [MemberData(nameof(RepositoryTests.DeleteMemberData), MemberType = typeof(RepositoryTests))]
        public void Delete_Success<TKey>(
            TKey defaultKey,
            FakeEntity<TKey> entity,
            Collection<FakeEntity<TKey>> existingEntities,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange
            DbContext
                .Set<FakeEntity<TKey>>()
                .AddRange(existingEntities);

            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();

            var repository = GetRepository<TKey>();

            // Act
            repository.Delete(entity);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            dbContextResult.Should().BeEquivalentTo(expectedEntities);
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.DeleteMemberData), MemberType = typeof(RepositoryTests))]
        public void Delete_WhenEntityAttached_Success<TKey>(
            TKey defaultKey,
            FakeEntity<TKey> entity,
            Collection<FakeEntity<TKey>> existingEntities,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange
            DbContext
                .Set<FakeEntity<TKey>>()
                .AddRange(existingEntities);

            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();

            DbContext.Attach(entity);

            var repository = GetRepository<TKey>();

            // Act
            repository.Delete(entity);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            dbContextResult.Should().BeEquivalentTo(expectedEntities);
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.DeleteListMemberData), MemberType = typeof(RepositoryTests))]
        public void DeleteList_Success<TKey>(
            TKey defaultKey,
            Collection<FakeEntity<TKey>> entities,
            Collection<FakeEntity<TKey>> existingEntities,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange
            DbContext
                .Set<FakeEntity<TKey>>()
                .AddRange(existingEntities);

            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();

            var repository = GetRepository<TKey>();

            // Act
            repository.Delete(entities);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            dbContextResult.Should().BeEquivalentTo(expectedEntities);
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.DeleteListMemberData), MemberType = typeof(RepositoryTests))]
        public void DeleteList_WhenEntityAttached_Success<TKey>(
            TKey defaultKey,
            Collection<FakeEntity<TKey>> entities,
            Collection<FakeEntity<TKey>> existingEntities,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange
            DbContext
                .Set<FakeEntity<TKey>>()
                .AddRange(existingEntities);

            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();

            DbContext.AttachRange(entities);

            var repository = GetRepository<TKey>();

            // Act
            repository.Delete(entities);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            dbContextResult.Should().BeEquivalentTo(expectedEntities);
        }
    }
}
