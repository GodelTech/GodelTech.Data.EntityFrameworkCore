using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests
{
    public partial class RepositoryTests
    {
        public static IEnumerable<object[]> InsertMemberData =>
            new Collection<object[]>
            {
                // Guid
                new object[]
                {
                    default(Guid),
                    new FakeEntity<Guid>
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000001"),
                        Name = "Test Name"
                    },
                    new Collection<FakeEntity<Guid>>
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Name = "Test Name"
                        }
                    }
                },
                // int
                new object[]
                {
                    default(int),
                    new FakeEntity<int>
                    {
                        Id = 2,
                        Name = "Test Name"
                    },
                    new Collection<FakeEntity<int>>
                    {
                        new FakeEntity<int>
                        {
                            Id = 2,
                            Name = "Test Name"
                        }
                    }
                },
                // string
                new object[]
                {
                    string.Empty,
                    new FakeEntity<string>
                    {
                        Id = "Test Id",
                        Name = "Test Name"
                    },
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id",
                            Name = "Test Name"
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(InsertMemberData))]
        public void Insert_Success<TKey>(
            TKey defaultKey,
            FakeEntity<TKey> entity,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange & Act
            var result = _unitOfWork
                .GetFakeTypeEntityRepository<TKey>()
                .Insert(entity);

            _unitOfWork.Commit();

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

        public static IEnumerable<object[]> InsertListMemberData =>
            new Collection<object[]>
            {
                // Guid
                new object[]
                {
                    default(Guid),
                    new Collection<FakeEntity<Guid>>(),
                    new Collection<FakeEntity<Guid>>()
                },
                new object[]
                {
                    default(Guid),
                    new Collection<FakeEntity<Guid>>()
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<Guid>>()
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Name = "Test Name"
                        }
                    }
                },
                new object[]
                {
                    default(Guid),
                    new Collection<FakeEntity<Guid>>()
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            Name = "Test Name One"
                        },
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000003"),
                            Name = "Test Name Two"
                        }
                    },
                    new Collection<FakeEntity<Guid>>()
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            Name = "Test Name One"
                        },
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000003"),
                            Name = "Test Name Two"
                        }
                    }
                },
                // int
                new object[]
                {
                    default(int),
                    new Collection<FakeEntity<int>>(),
                    new Collection<FakeEntity<int>>()
                },
                new object[]
                {
                    default(int),
                    new Collection<FakeEntity<int>>
                    {
                        new FakeEntity<int>
                        {
                            Id = 2,
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<int>>
                    {
                        new FakeEntity<int>
                        {
                            Id = 2,
                            Name = "Test Name"
                        }
                    }
                },
                new object[]
                {
                    default(int),
                    new Collection<FakeEntity<int>>
                    {
                        new FakeEntity<int>
                        {
                            Id = 3,
                            Name = "Test Name One"
                        },
                        new FakeEntity<int>
                        {
                            Id = 4,
                            Name = "Test Name Two"
                        }
                    },
                    new Collection<FakeEntity<int>>
                    {
                        new FakeEntity<int>
                        {
                            Id = 3,
                            Name = "Test Name One"
                        },
                        new FakeEntity<int>
                        {
                            Id = 4,
                            Name = "Test Name Two"
                        }
                    }
                },
                // string
                new object[]
                {
                    string.Empty,
                    new Collection<FakeEntity<string>>(),
                    new Collection<FakeEntity<string>>()
                },
                new object[]
                {
                    string.Empty,
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id",
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id",
                            Name = "Test Name"
                        }
                    }
                },
                new object[]
                {
                    string.Empty,
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id One",
                            Name = "Test Name One"
                        },
                        new FakeEntity<string>
                        {
                            Id = "Test Id Two",
                            Name = "Test Name Two"
                        }
                    },
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id One",
                            Name = "Test Name One"
                        },
                        new FakeEntity<string>
                        {
                            Id = "Test Id Two",
                            Name = "Test Name Two"
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(InsertListMemberData))]
        public void InsertList_Success<TKey>(
            TKey defaultKey,
            Collection<FakeEntity<TKey>> entities,
            Collection<FakeEntity<TKey>> expectedEntities)
        {
            // Arrange & Act
            _unitOfWork
                .GetFakeTypeEntityRepository<TKey>()
                .Insert(entities);

            _unitOfWork.Commit();

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            Assert.Equal(expectedEntities, dbContextResult, new FakeEntityEqualityComparer<TKey>());
        }
    }
}