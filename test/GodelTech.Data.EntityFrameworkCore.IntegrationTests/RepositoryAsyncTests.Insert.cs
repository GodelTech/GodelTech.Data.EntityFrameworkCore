using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests
{
    public sealed partial class RepositoryAsyncTests : IDisposable
    {
        [Theory]
        [MemberData(nameof(RepositoryTests.InsertMemberData), MemberType = typeof(RepositoryTests))]
        public async Task InsertAsync_Success<TKey>(
            TKey defaultKey,
            FakeEntity<TKey> entity,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange & Act
            var result = await _unitOfWork
                .GetFakeTypeEntityRepository<TKey>()
                .InsertAsync(entity);

            await _unitOfWork.CommitAsync();

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextEntityResult = await DbContext
                .Set<FakeEntity<TKey>>()
                .SingleAsync(x => x.Id.Equals(entity.Id));

            Assert.Equal(entity, result);
            Assert.Equal(dbContextEntityResult, result, new FakeEntityEqualityComparer<TKey>());

            var dbContextResult = await DbContext
                .Set<FakeEntity<TKey>>()
                .ToListAsync();

            Assert.Equal(expectedEntities, dbContextResult, new FakeEntityEqualityComparer<TKey>());
        }

        [Theory]
        [MemberData(nameof(RepositoryTests.InsertListMemberData), MemberType = typeof(RepositoryTests))]
        public async Task InsertListAsync_Success<TKey>(
            TKey defaultKey,
            Collection<FakeEntity<TKey>> entities,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange & Act
            await _unitOfWork
                .GetFakeTypeEntityRepository<TKey>()
                .InsertAsync(entities);

            await _unitOfWork.CommitAsync();

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = await DbContext
                .Set<FakeEntity<TKey>>()
                .ToListAsync();

            Assert.Equal(expectedEntities, dbContextResult, new FakeEntityEqualityComparer<TKey>());
        }
    }
}
