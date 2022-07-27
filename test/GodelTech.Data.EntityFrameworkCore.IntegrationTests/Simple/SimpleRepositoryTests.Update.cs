using System.Collections.ObjectModel;
using System.Linq;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Simple
{
    public partial class SimpleRepositoryTests
    {
        [Theory]
        [MemberData(nameof(RepositoryTests.UpdateMemberData), MemberType = typeof(RepositoryTests))]
        public void Update_Success<TKey>(
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
            var result = repository.Update(entity);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextEntityResult = DbContext
                .Set<FakeEntity<TKey>>()
                .Single(x => x.Id.Equals(entity.Id));

            Assert.Equal(entity, result);
            Assert.Equal(dbContextEntityResult, result, new FakeEntityEqualityComparer<TKey>());

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            Assert.Equal(expectedEntities, dbContextResult, new FakeEntityEqualityComparer<TKey>());
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.UpdateMemberData), MemberType = typeof(RepositoryTests))]
        public void Update_WithStartTrackProperties_EntityNotMarkedAsModified<TKey>(
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
            var result = repository.Update(entity, true);

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextEntityResult = DbContext
                .Set<FakeEntity<TKey>>()
                .Single(x => x.Id.Equals(entity.Id));

            Assert.Equal(entity, result);
            Assert.Equal(dbContextEntityResult, result, new FakeEntityEqualityComparer<TKey>());

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            Assert.Equal(expectedEntities, dbContextResult, new FakeEntityEqualityComparer<TKey>());
        }
    }
}
