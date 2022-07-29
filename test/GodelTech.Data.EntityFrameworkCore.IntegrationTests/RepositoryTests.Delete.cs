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
        public static IEnumerable<object[]> DeleteMemberData =>
            new Collection<object[]>
            {
                // no item
                new object[]
                {
                    default(Guid),
                    new FakeEntity<Guid>
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000000"),
                        Name = "Test Name"
                    },
                    new Collection<FakeEntity<Guid>>
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Name = "Test Name"
                        }
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
                    },
                    new Collection<FakeEntity<Guid>>()
                },
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
                        },
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<Guid>>
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
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
                        Id = 1,
                        Name = "Test Name"
                    },
                    new Collection<FakeEntity<int>>
                    {
                        new FakeEntity<int>
                        {
                            Id = 1,
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<int>>()
                },
                new object[]
                {
                    default(int),
                    new FakeEntity<int>
                    {
                        Id = 1,
                        Name = "Test Name"
                    },
                    new Collection<FakeEntity<int>>
                    {
                        new FakeEntity<int>
                        {
                            Id = 1,
                            Name = "Test Name"
                        },
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
                // string
                new object[]
                {
                    string.Empty,
                    new FakeEntity<string>
                    {
                        Id = "Test Id One",
                        Name = "Test Name"
                    },
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id One",
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<string>>()
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntity<string>
                    {
                        Id = "Test Id One",
                        Name = "Test Name"
                    },
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id One",
                            Name = "Test Name"
                        },
                        new FakeEntity<string>
                        {
                            Id = "Test Id Two",
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id Two",
                            Name = "Test Name"
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(DeleteMemberData))]
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

            // Act
            _unitOfWork
                .GetFakeTypeEntityRepository<TKey>()
                .Delete(entity);

            _unitOfWork.Commit();

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            Assert.Equal(dbContextResult, expectedEntities, new FakeEntityEqualityComparer<TKey>());
        }

        [Theory]
        [MemberData(nameof(DeleteMemberData))]
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

            // Act
            _unitOfWork
                .GetFakeTypeEntityRepository<TKey>()
                .Delete(entity);

            _unitOfWork.Commit();

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            Assert.Equal(dbContextResult, expectedEntities, new FakeEntityEqualityComparer<TKey>());
        }

        public static IEnumerable<object[]> DeleteListMemberData =>
            new Collection<object[]>
            {
                // no item
                new object[]
                {
                    default(Guid),
                    new Collection<FakeEntity<Guid>>
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000000"),
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<Guid>>
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Name = "Test Name"
                        }
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
                // Guid
                new object[]
                {
                    default(Guid),
                    new Collection<FakeEntity<Guid>>
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<Guid>>
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<Guid>>()
                },
                new object[]
                {
                    default(Guid),
                    new Collection<FakeEntity<Guid>>
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<Guid>>
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Name = "Test Name"
                        },
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<Guid>>
                    {
                        new FakeEntity<Guid>
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            Name = "Test Name"
                        }
                    }
                },
                // int
                new object[]
                {
                    default(int),
                    new Collection<FakeEntity<int>>
                    {
                        new FakeEntity<int>
                        {
                            Id = 1,
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<int>>
                    {
                        new FakeEntity<int>
                        {
                            Id = 1,
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<int>>()
                },
                new object[]
                {
                    default(int),
                    new Collection<FakeEntity<int>>
                    {
                        new FakeEntity<int>
                        {
                            Id = 1,
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<int>>
                    {
                        new FakeEntity<int>
                        {
                            Id = 1,
                            Name = "Test Name"
                        },
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
                // string
                new object[]
                {
                    string.Empty,
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id One",
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id One",
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<string>>()
                },
                new object[]
                {
                    string.Empty,
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id One",
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id One",
                            Name = "Test Name"
                        },
                        new FakeEntity<string>
                        {
                            Id = "Test Id Two",
                            Name = "Test Name"
                        }
                    },
                    new Collection<FakeEntity<string>>
                    {
                        new FakeEntity<string>
                        {
                            Id = "Test Id Two",
                            Name = "Test Name"
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(DeleteListMemberData))]
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

            // Act
            _unitOfWork
                .GetFakeTypeEntityRepository<TKey>()
                .Delete(entities);

            _unitOfWork.Commit();

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            Assert.Equal(dbContextResult, expectedEntities, new FakeEntityEqualityComparer<TKey>());
        }

        [Theory]
        [MemberData(nameof(DeleteListMemberData))]
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

            // Act
            _unitOfWork
                .GetFakeTypeEntityRepository<TKey>()
                .Delete(entities);

            _unitOfWork.Commit();

            // Assert
            Assert.NotNull(defaultKey);

            var dbContextResult = DbContext
                .Set<FakeEntity<TKey>>()
                .ToList();

            Assert.Equal(dbContextResult, expectedEntities, new FakeEntityEqualityComparer<TKey>());
        }
    }
}
