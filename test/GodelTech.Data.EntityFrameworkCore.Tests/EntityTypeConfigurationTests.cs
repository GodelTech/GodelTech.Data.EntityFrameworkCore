using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public class EntityTypeConfigurationTests
    {
        public static IEnumerable<object[]> ConstructorMemberData =>
            new Collection<object[]>
            {
                // Guid
                new object[]
                {
                    new FakeEntityTypeConfiguration<IEntity<Guid>, Guid>(null),
                    null
                },
                new object[]
                {
                    new FakeEntityTypeConfiguration<IEntity<Guid>, Guid>(
                        string.Empty
                    ),
                    string.Empty
                },
                new object[]
                {
                    new FakeEntityTypeConfiguration<IEntity<Guid>, Guid>(
                        "Test SchemaName"
                    ),
                    "Test SchemaName"
                },
                // int
                new object[]
                {
                    new FakeEntityTypeConfiguration<IEntity<int>, int>(null),
                    null
                },
                new object[]
                {
                    new FakeEntityTypeConfiguration<IEntity<int>, int>(
                        string.Empty
                    ),
                    string.Empty
                },
                new object[]
                {
                    new FakeEntityTypeConfiguration<IEntity<int>, int>(
                        "Test SchemaName"
                    ),
                    "Test SchemaName"
                },
                // string
                new object[]
                {
                    new FakeEntityTypeConfiguration<IEntity<string>, string>(null),
                    null
                },
                new object[]
                {
                    new FakeEntityTypeConfiguration<IEntity<string>, string>(
                        string.Empty
                    ),
                    string.Empty
                },
                new object[]
                {
                    new FakeEntityTypeConfiguration<IEntity<string>, string>(
                        "Test SchemaName"
                    ),
                    "Test SchemaName"
                }
            };

        [Theory]
        [MemberData(nameof(ConstructorMemberData))]
        public void Constructor<TEntity, TKey>(
            EntityTypeConfiguration<TEntity, TKey> item,
            string expectedSchemaName)
            where TEntity : class, IEntity<TKey>
        {
            // Arrange & Act & Assert
            Assert.Equal(expectedSchemaName, item?.SchemaName);
        }
    }
}
