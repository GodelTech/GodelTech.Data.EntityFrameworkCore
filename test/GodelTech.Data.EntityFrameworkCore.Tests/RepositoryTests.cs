using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

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

        public static IEnumerable<object[]> QueryMemberData
        {
            get
            {
                var list = new List<object[]>();

                list.AddRange(NonPagedResultQueryMemberData);
                list.AddRange(PagedResultQueryMemberData);

                return list;
            }
        }

        public static IEnumerable<object[]> NonPagedResultQueryMemberData =>
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

        public static IEnumerable<object[]> PagedResultQueryMemberData =>
            new Collection<object[]>
            {
                // Guid
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
                }
                // int
                // string
            };

        [Theory]
        [MemberData(nameof(QueryMemberData))]
        public void Query_Success<TKey>(
            TKey defaultKey,
            QueryParameters<FakeEntity<TKey>, TKey> queryParameters,
            Collection<FakeEntity<TKey>> entities,
            IQueryable<FakeEntity<TKey>> queryableEntities,
            int filteredEntitiesCount)
        {
            // Arrange
            var expectedResult = queryableEntities.ToList();

            var repository = (FakeRepository<FakeEntity<TKey>, TKey>) GetRepository<FakeEntity<TKey>, TKey>(entities);

            // Act
            var result = repository.ExposedQuery(queryParameters).ToList();

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(filteredEntitiesCount, queryableEntities.ToList().Count);
            Assert.Equal(expectedResult, result, new FakeEntityEqualityComparer<TKey>());
        }

        [Theory]
        [MemberData(nameof(QueryMemberData))]
        public void QueryModel_Success<TKey>(
            TKey defaultKey,
            QueryParameters<FakeEntity<TKey>, TKey> queryParameters,
            Collection<FakeEntity<TKey>> entities,
            IQueryable<FakeEntity<TKey>> queryableEntities,
            int filteredEntitiesCount)
        {
            // Arrange
            var expectedResult = queryableEntities
                .Select(
                    x => new FakeModel<TKey>
                    {
                        Id = x.Id,
                        Name = x.Name
                    }
                )
                .ToList();

            _mockDataMapper
                .Setup(
                    x => x.Map<FakeModel<TKey>>(
                        It.Is<IQueryable<FakeEntity<TKey>>>(
                            y => !y
                                .Except(queryableEntities)
                                .Any()
                        )
                    )
                )
                .Returns(
                    queryableEntities
                        .Select(
                            x => new FakeModel<TKey>
                            {
                                Id = x.Id,
                                Name = x.Name
                            }
                        )
                );

            var repository = (FakeRepository<FakeEntity<TKey>, TKey>) GetRepository<FakeEntity<TKey>, TKey>(entities);

            // Act
            var result = repository.ExposedQuery<FakeModel<TKey>>(queryParameters).ToList();

            // Assert
            _mockDataMapper
                .Verify(
                    x => x.Map<FakeModel<TKey>>(
                        It.Is<IQueryable<FakeEntity<TKey>>>(
                            y => !y
                                .Except(queryableEntities)
                                .Any()
                        )
                    ),
                    Times.Once
                );

            Assert.NotNull(defaultKey);
            Assert.Equal(filteredEntitiesCount, queryableEntities.ToList().Count);
            Assert.Equal(expectedResult, result, new FakeModelEqualityComparer<TKey>());
        }

        [Theory]
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public void PagedResultQuery_WhenQueryParametersIsNull_ThrowsArgumentNullException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var repository = (FakeRepository<FakeEntity<TKey>, TKey>) GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = Assert.Throws<ArgumentNullException>(
                () => repository.ExposedPagedResultQuery(null)
            );
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public void PagedResultQuery_WhenQueryParametersPageIsNull_ThrowsArgumentException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var repository = (FakeRepository<FakeEntity<TKey>, TKey>) GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = Assert.Throws<ArgumentException>(
                () => repository.ExposedPagedResultQuery(
                    new QueryParameters<FakeEntity<TKey>, TKey>
                    {
                        Page = null
                    }
                )
            );
            Assert.Equal("Page can't be null. (Parameter 'queryParameters')", exception.Message);
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public void PagedResultQuery_WhenQueryParametersPageIsNotValid_ThrowsArgumentException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var repository = (FakeRepository<FakeEntity<TKey>, TKey>) GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = Assert.Throws<ArgumentException>(
                () => repository.ExposedPagedResultQuery(
                    new QueryParameters<FakeEntity<TKey>, TKey>
                    {
                        Page = new PageRule
                        {
                            Index = -1,
                            Size = -1
                        }
                    }
                )
            );
            Assert.Equal("Page is not valid. (Parameter 'queryParameters')", exception.Message);
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(PagedResultQueryMemberData))]
        public void PagedResultQuery_Success<TKey>(
            TKey defaultKey,
            QueryParameters<FakeEntity<TKey>, TKey> queryParameters,
            Collection<FakeEntity<TKey>> entities,
            IQueryable<FakeEntity<TKey>> queryableEntities,
            int filteredEntitiesCount)
        {
            // Arrange
            var expectedResult = queryableEntities.ToList();

            var repository = (FakeRepository<FakeEntity<TKey>, TKey>) GetRepository<FakeEntity<TKey>, TKey>(entities);

            // Act
            var result = repository.ExposedPagedResultQuery(queryParameters).ToList();

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(filteredEntitiesCount, queryableEntities.ToList().Count);
            Assert.Equal(expectedResult, result, new FakeEntityEqualityComparer<TKey>());
        }

        [Theory]
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public void PagedResultQueryModel_WhenQueryParametersIsNull_ThrowsArgumentNullException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var repository = (FakeRepository<FakeEntity<TKey>, TKey>) GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = Assert.Throws<ArgumentNullException>(
                () => repository.ExposedPagedResultQuery<FakeModel<TKey>>(null)
            );
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public void PagedResultQueryModel_WhenQueryParametersPageIsNull_ThrowsArgumentException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var repository = (FakeRepository<FakeEntity<TKey>, TKey>) GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = Assert.Throws<ArgumentException>(
                () => repository.ExposedPagedResultQuery<FakeModel<TKey>>(
                    new QueryParameters<FakeEntity<TKey>, TKey>
                    {
                        Page = null
                    }
                )
            );
            Assert.Equal("Page can't be null. (Parameter 'queryParameters')", exception.Message);
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(UnitOfWorkTests.TypesMemberData), MemberType = typeof(UnitOfWorkTests))]
        public void PagedResultQueryModel_WhenQueryParametersPageIsNotValid_ThrowsArgumentException<TKey>(
            TKey defaultKey)
        {
            // Arrange
            var repository = (FakeRepository<FakeEntity<TKey>, TKey>) GetRepository<FakeEntity<TKey>, TKey>(new List<FakeEntity<TKey>>());

            // Act & Assert
            Assert.NotNull(defaultKey);

            var exception = Assert.Throws<ArgumentException>(
                () => repository.ExposedPagedResultQuery<FakeModel<TKey>>(
                    new QueryParameters<FakeEntity<TKey>, TKey>
                    {
                        Page = new PageRule
                        {
                            Index = -1,
                            Size = -1
                        }
                    }
                )
            );
            Assert.Equal("Page is not valid. (Parameter 'queryParameters')", exception.Message);
            Assert.Equal("queryParameters", exception.ParamName);
        }

        [Theory]
        [MemberData(nameof(PagedResultQueryMemberData))]
        public void PagedResultQueryModel_Success<TKey>(
            TKey defaultKey,
            QueryParameters<FakeEntity<TKey>, TKey> queryParameters,
            Collection<FakeEntity<TKey>> entities,
            IQueryable<FakeEntity<TKey>> queryableEntities,
            int filteredEntitiesCount)
        {
            // Arrange
            var expectedResult = queryableEntities
                .Select(
                    x => new FakeModel<TKey>
                    {
                        Id = x.Id,
                        Name = x.Name
                    }
                )
                .ToList();

            _mockDataMapper
                .Setup(
                    x => x.Map<FakeModel<TKey>>(
                        It.Is<IQueryable<FakeEntity<TKey>>>(
                            y => !y
                                .Except(queryableEntities)
                                .Any()
                        )
                    )
                )
                .Returns(
                    queryableEntities
                        .Select(
                            x => new FakeModel<TKey>
                            {
                                Id = x.Id,
                                Name = x.Name
                            }
                        )
                );

            var repository = (FakeRepository<FakeEntity<TKey>, TKey>) GetRepository<FakeEntity<TKey>, TKey>(entities);

            // Act
            var result = repository.ExposedPagedResultQuery<FakeModel<TKey>>(queryParameters);

            // Assert
            _mockDataMapper
                .Verify(
                    x => x.Map<FakeModel<TKey>>(
                        It.Is<IQueryable<FakeEntity<TKey>>>(
                            y => !y
                                .Except(queryableEntities)
                                .Any()
                        )
                    ),
                    Times.Once
                );

            Assert.NotNull(defaultKey);
            Assert.Equal(filteredEntitiesCount, queryableEntities.ToList().Count);
            Assert.Equal(expectedResult, result, new FakeModelEqualityComparer<TKey>());
        }

        [Theory]
        [MemberData(nameof(QueryMemberData))]
        public void CountQuery_Success<TKey>(
            TKey defaultKey,
            QueryParameters<FakeEntity<TKey>, TKey> queryParameters,
            Collection<FakeEntity<TKey>> entities,
            IQueryable<FakeEntity<TKey>> queryableEntities,
            int filteredEntitiesCount)
        {
            // Arrange
            var query = entities.AsQueryable();

            if (queryParameters?.Filter?.Expression != null)
            {
                query = query.Where(queryParameters.Filter.Expression);
            }

            var expectedResult = query.Count();

            var repository = (FakeRepository<FakeEntity<TKey>, TKey>) GetRepository<FakeEntity<TKey>, TKey>(entities);

            // Act
            var result = repository.ExposedCountQuery(queryParameters).Count();

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(filteredEntitiesCount, queryableEntities.ToList().Count);
            Assert.Equal(expectedResult, result);
        }

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

            return new FakeRepository<TEntity, TKey>(
                _mockDbContext.Object,
                _mockDataMapper.Object
            );
        }
    }
}