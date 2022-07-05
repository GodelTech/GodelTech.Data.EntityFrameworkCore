using System.Collections.Generic;
using System.Collections.ObjectModel;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public class DbContextBaseTests
    {
        public static IEnumerable<object[]> ConstructorMemberData =>
            new Collection<object[]>
            {
                new object[]
                {
                    new FakeDbContext(
                        new DbContextOptions<FakeDbContext>(),
                        null
                    ),
                    null
                },
                new object[]
                {
                    new FakeDbContext(
                        new DbContextOptions<FakeDbContext>(),
                        string.Empty
                    ),
                    string.Empty
                },
                new object[]
                {
                    new FakeDbContext(
                        new DbContextOptions<FakeDbContext>(),
                        "Test SchemaName"
                    ),
                    "Test SchemaName"
                }
            };

        [Theory]
        [MemberData(nameof(ConstructorMemberData))]
        public void Constructor(
            DbContextBase item,
            string expectedSchemaName)
        {
            // Arrange & Act & Assert
            Assert.Equal(expectedSchemaName, item?.SchemaName);
        }
    }
}
