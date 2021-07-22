using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public partial class RepositoryTests
    {
        private static readonly Collection<FakeEntity<Guid>> GuidEntities = new Collection<FakeEntity<Guid>>
        {
            new FakeEntity<Guid> { Id = new Guid("00000000-0000-0000-0000-000000000001"), Name = "Test Name One" },
            new FakeEntity<Guid> { Id = new Guid("00000000-0000-0000-0000-000000000002"), Name = "Test Name Two" },
            new FakeEntity<Guid> { Id = new Guid("00000000-0000-0000-0000-000000000003"), Name = "Test Name Three" },
            new FakeEntity<Guid> { Id = new Guid("00000000-0000-0000-0000-000000000004"), Name = "Z Name Four" },
            new FakeEntity<Guid> { Id = new Guid("00000000-0000-0000-0000-000000000005"), Name = "A Name Five" },
            new FakeEntity<Guid> { Id = new Guid("00000000-0000-0000-0000-000000000006"), Name = "Test Name" },
            new FakeEntity<Guid> { Id = new Guid("00000000-0000-0000-0000-000000000007"), Name = "Other Test Name" },
            new FakeEntity<Guid> { Id = new Guid("00000000-0000-0000-0000-000000000008"), Name = "Test Name" },
            new FakeEntity<Guid> { Id = new Guid("00000000-0000-0000-0000-000000000009"), Name = "Test Name" },
            new FakeEntity<Guid> { Id = new Guid("00000000-0000-0000-0000-000000000010"), Name = "New Test Name" }
        };

        private readonly Mock<DbContext> _mockDbContext;
        private readonly Mock<IDataMapper> _mockDataMapper;

        public RepositoryTests()
        {
            _mockDbContext = new Mock<DbContext>(MockBehavior.Strict);
            _mockDataMapper = new Mock<IDataMapper>(MockBehavior.Strict);
        }

        public static IEnumerable<object[]> QueryMemberData =>
            new Collection<object[]>
            {
                // Guid
                new object[]
                {
                    default(Guid),
                    null,
                    new Collection<FakeEntity<Guid>>(),
                    new Collection<FakeEntity<Guid>>()
                        .AsQueryable(),
                    0
                },
                new object[]
                {
                    default(Guid),
                    new QueryParameters<FakeEntity<Guid>, Guid>(),
                    new Collection<FakeEntity<Guid>>(),
                    new Collection<FakeEntity<Guid>>()
                        .AsQueryable(),
                    0
                },
                new object[]
                {
                    default(Guid),
                    null,
                    GuidEntities,
                    GuidEntities
                        .AsQueryable(),
                    10
                },
                new object[]
                {
                    default(Guid),
                    new QueryParameters<FakeEntity<Guid>, Guid>(),
                    GuidEntities,
                    GuidEntities
                        .AsQueryable(),
                    10
                },
                new object[]
                {
                    default(Guid),
                    new QueryParameters<FakeEntity<Guid>, Guid>
                    {
                        Filter = new FilterRule<FakeEntity<Guid>, Guid>
                        {
                            Expression = x => x.Name == "Test Name"
                        }
                    },
                    GuidEntities,
                    GuidEntities
                        .Where(x => x.Name == "Test Name")
                        .AsQueryable(),
                    3
                },
                new object[]
                {
                    default(Guid),
                    new QueryParameters<FakeEntity<Guid>, Guid>
                    {
                        Sort = new SortRule<FakeEntity<Guid>, Guid>
                        {
                            SortOrder = SortOrder.Ascending,
                            Expression = x => x.Name
                        }
                    },
                    GuidEntities,
                    GuidEntities
                        .OrderBy(x => x.Name)
                        .AsQueryable(),
                    10
                },
                new object[]
                {
                    default(Guid),
                    new QueryParameters<FakeEntity<Guid>, Guid>
                    {
                        Sort = new SortRule<FakeEntity<Guid>, Guid>
                        {
                            SortOrder = SortOrder.Descending,
                            Expression = x => x.Name
                        }
                    },
                    GuidEntities,
                    GuidEntities
                        .OrderByDescending(x => x.Name)
                        .AsQueryable(),
                    10
                },
                new object[]
                {
                    default(Guid),
                    new QueryParameters<FakeEntity<Guid>, Guid>
                    {
                        Page = new PageRule
                        {
                            Index = 2,
                            Size = 4
                        }
                    },
                    GuidEntities,
                    GuidEntities
                        .Skip(4 * 2)
                        .Take(4)
                        .AsQueryable(),
                    2
                },
                new object[]
                {
                    default(Guid),
                    new QueryParameters<FakeEntity<Guid>, Guid>
                    {
                        Filter = new FilterRule<FakeEntity<Guid>, Guid>
                        {
                            Expression = x => x.Name == "Test Name"
                        },
                        Sort = new SortRule<FakeEntity<Guid>, Guid>
                        {
                            SortOrder = SortOrder.Ascending,
                            Expression = x => x.Name
                        }
                    },
                    GuidEntities,
                    GuidEntities
                        .Where(x => x.Name == "Test Name")
                        .OrderBy(x => x.Name)
                        .AsQueryable(),
                    3
                },
                new object[]
                {
                    default(Guid),
                    new QueryParameters<FakeEntity<Guid>, Guid>
                    {
                        Filter = new FilterRule<FakeEntity<Guid>, Guid>
                        {
                            Expression = x => x.Name == "Test Name"
                        },
                        Sort = new SortRule<FakeEntity<Guid>, Guid>
                        {
                            SortOrder = SortOrder.Descending,
                            Expression = x => x.Name
                        }
                    },
                    GuidEntities,
                    GuidEntities
                        .Where(x => x.Name == "Test Name")
                        .OrderByDescending(x => x.Name)
                        .AsQueryable(),
                    3
                },
                new object[]
                {
                    default(Guid),
                    new QueryParameters<FakeEntity<Guid>, Guid>
                    {
                        Filter = new FilterRule<FakeEntity<Guid>, Guid>
                        {
                            Expression = x => x.Name == "Test Name"
                        },
                        Page = new PageRule
                        {
                            Index = 1,
                            Size = 2
                        }
                    },
                    GuidEntities,
                    GuidEntities
                        .Where(x => x.Name == "Test Name")
                        .Skip(2 * 1)
                        .Take(2)
                        .AsQueryable(),
                    1
                },
                new object[]
                {
                    default(Guid),
                    new QueryParameters<FakeEntity<Guid>, Guid>
                    {
                        Sort = new SortRule<FakeEntity<Guid>, Guid>
                        {
                            SortOrder = SortOrder.Ascending,
                            Expression = x => x.Name
                        },
                        Page = new PageRule
                        {
                            Index = 2,
                            Size = 4
                        }
                    },
                    GuidEntities,
                    GuidEntities
                        .OrderBy(x => x.Name)
                        .Skip(4 * 2)
                        .Take(4)
                        .AsQueryable(),
                    2
                },
                new object[]
                {
                    default(Guid),
                    new QueryParameters<FakeEntity<Guid>, Guid>
                    {
                        Sort = new SortRule<FakeEntity<Guid>, Guid>
                        {
                            SortOrder = SortOrder.Descending,
                            Expression = x => x.Name
                        },
                        Page = new PageRule
                        {
                            Index = 2,
                            Size = 4
                        }
                    },
                    GuidEntities,
                    GuidEntities
                        .OrderByDescending(x => x.Name)
                        .Skip(4 * 2)
                        .Take(4)
                        .AsQueryable(),
                    2
                },
                // int
                new object[]
                {
                    default(int),
                    null,
                    new Collection<FakeEntity<int>>(),
                    new Collection<FakeEntity<int>>()
                        .AsQueryable(),
                    0
                },
                // string
                new object[]
                {
                    string.Empty,
                    null,
                    new Collection<FakeEntity<string>>(),
                    new Collection<FakeEntity<string>>()
                        .AsQueryable(),
                    0
                }
            };

        private Repository<TEntity, TKey> GetRepository<TEntity, TKey>(ICollection<TEntity> entities)
            where TEntity : class, IEntity<TKey>
        {
            var mockDbSet = new Mock<DbSet<TEntity>>(MockBehavior.Strict);

            mockDbSet.As<IQueryable<TEntity>>().Setup(x => x.Provider).Returns(entities.AsQueryable().Provider);
            mockDbSet.As<IQueryable<TEntity>>().Setup(x => x.Expression).Returns(entities.AsQueryable().Expression);
            mockDbSet.As<IQueryable<TEntity>>().Setup(x => x.ElementType).Returns(entities.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(entities.AsQueryable().GetEnumerator());

            _mockDbContext
                .Setup(x => x.Set<TEntity>())
                .Returns(() => mockDbSet.Object);

            return new Repository<TEntity, TKey>(
                _mockDbContext.Object,
                _mockDataMapper.Object
            );
        }
    }
}