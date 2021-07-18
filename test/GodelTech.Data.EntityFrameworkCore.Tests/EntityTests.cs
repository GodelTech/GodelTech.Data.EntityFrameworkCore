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
    public class EntityTests
    {
        private static readonly IEntity<Guid> ReferenceGuidEntity = new FakeEntity<Guid>();
        private static readonly IEntity<int> ReferenceIntEntity = new FakeEntity<int>();
        private static readonly IEntity<string> ReferenceStringEntity = new FakeEntity<string>();

        [Fact]
        public void Id_HasKeyAttribute()
        {
            // Arrange & Act & Assert
            var type = typeof(Entity<>);
            
            var property = type.GetProperty("Id");
            Assert.NotNull(property);

            var attribute = property.GetCustomAttribute<KeyAttribute>();
            Assert.NotNull(attribute);
        }

        [Fact]
        public void Id_HasDatabaseGeneratedAttribute()
        {
            // Arrange & Act & Assert
            var type = typeof(Entity<>);

            var property = type.GetProperty("Id");
            Assert.NotNull(property);

            var attribute = property.GetCustomAttribute<DatabaseGeneratedAttribute>();
            Assert.NotNull(attribute);
            Assert.Equal(DatabaseGeneratedOption.Identity, attribute.DatabaseGeneratedOption);
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
                    new FakeEntity<Guid>(),
                    false
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntity<Guid>(),
                    null,
                    false
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntity<Guid>(),
                    new FakeEntity<Guid>(),
                    true
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntity<Guid>(),
                    new FakeEntity<Guid>
                    {
                        Id = new Guid("762440ed-9876-4805-b00c-4ae53ba734a4")
                    },
                    false
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntity<Guid>
                    {
                        Id = new Guid("762440ed-9876-4805-b00c-4ae53ba734a4")
                    },
                    new FakeEntity<Guid>(),
                    false
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntity<Guid>
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000001")
                    },
                    new FakeEntity<Guid>
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
                    new FakeEntity<int>(),
                    false
                },
                new object[]
                {
                    default(int),
                    new FakeEntity<int>(),
                    null,
                    false
                },
                new object[]
                {
                    default(int),
                    new FakeEntity<int>(),
                    new FakeEntity<int>(),
                    true
                },
                new object[]
                {
                    default(int),
                    new FakeEntity<int>(),
                    new FakeEntity<int>
                    {
                        Id = 99
                    },
                    false
                },
                new object[]
                {
                    default(int),
                    new FakeEntity<int>
                    {
                        Id = 99
                    },
                    new FakeEntity<int>(),
                    false
                },
                new object[]
                {
                    default(int),
                    new FakeEntity<int>
                    {
                        Id = 1
                    },
                    new FakeEntity<int>
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
                    new FakeEntity<string>(),
                    false
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntity<string>(),
                    null,
                    false
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntity<string>
                    {
                        Id = string.Empty
                    },
                    new FakeEntity<string>
                    {
                        Id = string.Empty
                    },
                    true
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntity<string>
                    {
                        Id = string.Empty
                    },
                    new FakeEntity<string>
                    {
                        Id = "Test Id"
                    },
                    false
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntity<string>
                    {
                        Id = "Test Id"
                    },
                    new FakeEntity<string>(),
                    false
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntity<string>
                    {
                        Id = "Test Id"
                    },
                    new FakeEntity<string>
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
            var result = new FakeEntity<TKey>().Equals(x, y);

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
                    new FakeEntity<Guid>(),
                    0
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntity<Guid>
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
                    new FakeEntity<int>(),
                    0
                },
                new object[]
                {
                    default(int),
                    new FakeEntity<int>
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
                    new FakeEntity<string>
                    {
                        Id = string.Empty
                    },
                    string.Empty.GetHashCode(StringComparison.Ordinal)
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntity<string>
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
            var result = new FakeEntity<TKey>().GetHashCode(entity);

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(expectedResult, result);
        }
    }
}