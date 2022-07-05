using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public class EntityNoneDatabaseGeneratedIdentifierTests
    {
        private static readonly IEntity<Guid> ReferenceGuidEntity = new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>();
        private static readonly IEntity<int> ReferenceIntEntity = new FakeEntityNoneDatabaseGeneratedIdentifier<int>();
        private static readonly IEntity<string> ReferenceStringEntity = new FakeEntityNoneDatabaseGeneratedIdentifier<string>();

        [Fact]
        public void Id_HasKeyAttribute()
        {
            // Arrange & Act & Assert
            var type = typeof(EntityNoneDatabaseGeneratedIdentifier<>);

            var property = type.GetProperty("Id");
            Assert.NotNull(property);

            var attribute = property.GetCustomAttribute<KeyAttribute>();
            Assert.NotNull(attribute);
        }

        [Fact]
        public void Id_HasDatabaseGeneratedAttribute()
        {
            // Arrange & Act & Assert
            var type = typeof(EntityNoneDatabaseGeneratedIdentifier<>);

            var property = type.GetProperty("Id");
            Assert.NotNull(property);

            var attribute = property.GetCustomAttribute<DatabaseGeneratedAttribute>();
            Assert.NotNull(attribute);
            Assert.Equal(DatabaseGeneratedOption.None, attribute.DatabaseGeneratedOption);
        }

        public static IEnumerable<object[]> EqualsMemberData =>
            new Collection<object[]>
            {
                // Guid
                new object[]
                {
                    default(Guid),
                    null,
                    null,
                    true
                },
                new object[]
                {
                    default(Guid),
                    null,
                    ReferenceGuidEntity,
                    false
                },
                new object[]
                {
                    default(Guid),
                    ReferenceGuidEntity,
                    null,
                    false
                },
                new object[]
                {
                    default(Guid),
                    ReferenceGuidEntity,
                    ReferenceGuidEntity,
                    true
                },
                new object[]
                {
                    default(Guid),
                    null,
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>(),
                    false
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>(),
                    null,
                    false
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>(),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>(),
                    true
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>(),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>
                    {
                        Id = new Guid("762440ed-9876-4805-b00c-4ae53ba734a4")
                    },
                    false
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>
                    {
                        Id = new Guid("762440ed-9876-4805-b00c-4ae53ba734a4")
                    },
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>(),
                    false
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000001")
                    },
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000001")
                    },
                    true
                },
                // int
                new object[]
                {
                    default(int),
                    null,
                    null,
                    true
                },
                new object[]
                {
                    default(int),
                    null,
                    ReferenceIntEntity,
                    false
                },
                new object[]
                {
                    default(int),
                    ReferenceIntEntity,
                    null,
                    false
                },
                new object[]
                {
                    default(int),
                    ReferenceIntEntity,
                    ReferenceIntEntity,
                    true
                },
                new object[]
                {
                    default(int),
                    null,
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>(),
                    false
                },
                new object[]
                {
                    default(int),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>(),
                    null,
                    false
                },
                new object[]
                {
                    default(int),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>(),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>(),
                    true
                },
                new object[]
                {
                    default(int),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>(),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>
                    {
                        Id = 99
                    },
                    false
                },
                new object[]
                {
                    default(int),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>
                    {
                        Id = 99
                    },
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>(),
                    false
                },
                new object[]
                {
                    default(int),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>
                    {
                        Id = 1
                    },
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>
                    {
                        Id = 1
                    },
                    true
                },
                // string
                new object[]
                {
                    string.Empty,
                    null,
                    null,
                    true
                },
                new object[]
                {
                    string.Empty,
                    null,
                    ReferenceStringEntity,
                    false
                },
                new object[]
                {
                    string.Empty,
                    ReferenceStringEntity,
                    null,
                    false
                },
                new object[]
                {
                    string.Empty,
                    ReferenceStringEntity,
                    ReferenceStringEntity,
                    true
                },
                new object[]
                {
                    string.Empty,
                    null,
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>(),
                    false
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>(),
                    null,
                    false
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>
                    {
                        Id = string.Empty
                    },
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>
                    {
                        Id = string.Empty
                    },
                    true
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>
                    {
                        Id = string.Empty
                    },
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>
                    {
                        Id = "Test Id"
                    },
                    false
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>
                    {
                        Id = "Test Id"
                    },
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>(),
                    false
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>
                    {
                        Id = "Test Id"
                    },
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>
                    {
                        Id = "Test Id"
                    },
                    true
                }
            };

        [Theory]
        [MemberData(nameof(EqualsMemberData))]
        public void Equals_Success<TKey>(
            TKey defaultKey,
            IEntity<TKey> x,
            IEntity<TKey> y,
            bool expectedResult)
        {
            // Arrange & Act
            var result = new FakeEntityNoneDatabaseGeneratedIdentifier<TKey>().Equals(x, y);

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(expectedResult, result);
        }

        public static IEnumerable<object[]> GetHashCodeMemberData =>
            new Collection<object[]>
            {
                // Guid
                new object[]
                {
                    default(Guid),
                    null,
                    0
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>(),
                    0
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<Guid>
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000001")
                    },
                    16777216
                },
                // int
                new object[]
                {
                    default(int),
                    null,
                    0
                },
                new object[]
                {
                    default(int),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>(),
                    0
                },
                new object[]
                {
                    default(int),
                    new FakeEntityNoneDatabaseGeneratedIdentifier<int>
                    {
                        Id = 99
                    },
                    99
                },
                // string
                new object[]
                {
                    string.Empty,
                    null,
                    0
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>
                    {
                        Id = string.Empty
                    },
                    string.Empty.GetHashCode(StringComparison.Ordinal)
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntityNoneDatabaseGeneratedIdentifier<string>
                    {
                        Id = "Test Id"
                    },
                    "Test Id".GetHashCode(StringComparison.Ordinal)
                }
            };

        [Theory]
        [MemberData(nameof(GetHashCodeMemberData))]
        public void GetHashCode_Success<TKey>(
            TKey defaultKey,
            IEntity<TKey> entity,
            int expectedResult)
        {
            // Arrange & Act
            var result = new FakeEntityNoneDatabaseGeneratedIdentifier<TKey>().GetHashCode(entity);

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(expectedResult, result);
        }
    }
}
