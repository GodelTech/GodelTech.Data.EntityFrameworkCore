using System;
using System.Collections.ObjectModel;
using System.Linq;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using GodelTech.Data.EntityFrameworkCore.Simple;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Simple.Extensions
{
    public partial class SimpleRepositoryExtensionsTests
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
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            DbContext
                .Set<FakeEntity<TKey>>()
                .AddRange(existingEntities);

            DbContext.SaveChanges();
            DbContext.ChangeTracker.Clear();

            var repository = GetRepository<TKey>();

            // Act
            SimpleRepositoryExtensions.Delete(repository, entity.Id);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            Assert.Equal(dbContextResult, expectedEntities, new FakeEntityEqualityComparer<TKey>());
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
            SimpleRepositoryExtensions.Delete(repository, entities.Select(x => x.Id));

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            Assert.Equal(dbContextResult, expectedEntities, new FakeEntityEqualityComparer<TKey>());
        }
    }
}
