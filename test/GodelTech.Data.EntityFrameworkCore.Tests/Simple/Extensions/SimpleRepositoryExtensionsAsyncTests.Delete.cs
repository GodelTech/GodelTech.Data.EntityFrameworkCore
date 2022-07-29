using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GodelTech.Data.EntityFrameworkCore.Simple;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Simple.Extensions
{
    public partial class SimpleRepositoryExtensionsAsyncTests
    {
        [Theory]
        [MemberData(nameof(TypesMemberData))]
        public async Task DeleteAsync_ByIdWhenRepositoryIsNull_ThrowsArgumentNullException<TKey>(TKey defaultKey)
        {
            // Arrange & Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => SimpleRepositoryExtensions.DeleteAsync<IEntity<TKey>, TKey>(null, defaultKey)
            );

            Assert.Equal("repository", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(TypesMemberData))]
        public async Task DeleteAsync_ByIdsWhenRepositoryIsNull_ThrowsArgumentNullException<TKey>(TKey defaultKey)
        {
            // Arrange & Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(
                () => SimpleRepositoryExtensions.DeleteAsync<IEntity<TKey>, TKey>(
                    null,
                    new List<TKey>
                    {
                        defaultKey
                    }
                )
            );

            Assert.Equal("repository", exception.ParamName);
        }
    }
}
