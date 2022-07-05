using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests
{
    public partial class RepositoryTests
    {
        public static IEnumerable<object[]> UpdateMemberData =>
            new Collection<object[]>
            {
                // Guid
                new object[]
                {
                    default(Guid),
                    new FakeEntity<Guid>
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000001"),
                        Name = "Test Name New"
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
                            Name = "Test Name New"
                        }
                    }
                },
                new object[]
                {
                    default(Guid),
                    new FakeEntity<Guid>
                    {
                        Id = new Guid("00000000-0000-0000-0000-000000000001"),
                        Name = "Test Name New"
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
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Name = "Test Name New"
                        },
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
                        Name = "Test Name New"
                    },
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
                            Name = "Test Name New"
                        }
                    }
                },
                new object[]
                {
                    default(int),
                    new FakeEntity<int>
                    {
                        Id = 1,
                        Name = "Test Name New"
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
                            Id = 1,
                            Name = "Test Name New"
                        },
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
                        Name = "Test Name New"
                    },
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
                            Name = "Test Name New"
                        }
                    }
                },
                new object[]
                {
                    string.Empty,
                    new FakeEntity<string>
                    {
                        Id = "Test Id One",
                        Name = "Test Name New"
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
                            Id = "Test Id One",
                            Name = "Test Name New"
                        },
                        new FakeEntity<string>
                        {
                            Id = "Test Id Two",
                            Name = "Test Name"
                        }
                    }
                }
            };

        [Theory]
        [MemberData(nameof(UpdateMemberData))]
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

            // Act
            var result = _unitOfWork
                .GetFakeTypeEntityRepository<TKey>()
                .Update(entity);

            var entry = DbContext.Entry(result);
            Assert.Equal(EntityState.Modified, entry.State);

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

        [Theory]
        [MemberData(nameof(UpdateMemberData))]
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

            // Act
            var result = _unitOfWork
                .GetFakeTypeEntityRepository<TKey>()
                .Update(entity, true);

            var entry = DbContext.Entry(result);
            Assert.Equal(EntityState.Unchanged, entry.State);

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
    }
}
