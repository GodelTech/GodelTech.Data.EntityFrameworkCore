using System;
using System.Collections.Generic;
using System.Linq;
using Aurochses.Xunit;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public partial class RepositoryTests : IClassFixture<RepositoryFixture>
    {
        private readonly RepositoryFixture _fixture;

        public RepositoryTests(RepositoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Inherit_IRepository()
        {
            // Arrange
            var mockDbContext = new Mock<DbContext>(MockBehavior.Strict);

            // Act
            var repository = new Repository<Entity<int>, int>(mockDbContext.Object);

            // Assert
            Assert.IsAssignableFrom<IRepository<Entity<int>, int>>(repository);
        }

        private static DbSet<FakeEntity> DbSet => new FakeDbContext(new DbContextOptionsBuilder<FakeDbContext>().UseInMemoryDatabase($"{nameof(RepositoryTests)}{Guid.NewGuid():N}").Options, "dbo").Set<FakeEntity>();

        public static IEnumerable<object[]> QueryParametersMemberData => new[]
        {
            new object[]
            {
                null,
                DbSet.AsQueryable(),
                DbSet.AsQueryable()
            },
            new object[]
            {
                new QueryParameters<FakeEntity, int>(),
                DbSet.AsQueryable(),
                DbSet.AsQueryable()
            },
            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Filter = new FilterRule<FakeEntity, int>(),
                    Sort = new SortRule<FakeEntity, int>(),
                    Page = new PageRule()
                },
                DbSet.AsQueryable(),
                DbSet.AsQueryable()
            },

            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Filter = new FilterRule<FakeEntity, int>
                    {
                        Expression = x => x.Id == 1
                    }
                },
                DbSet.AsQueryable().Where(x => x.Id == 1),
                DbSet.AsQueryable().Where(x => x.Id == 1)
            },

            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Sort = new SortRule<FakeEntity, int>
                    {
                        Expression = x => x.Id
                    }
                },
                DbSet.AsQueryable().OrderBy(x => (object) x.Id),
                DbSet.AsQueryable()
            },
            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Sort = new SortRule<FakeEntity, int>
                    {
                        SortOrder = SortOrder.Ascending,
                        Expression = x => x.Id
                    }
                },
                DbSet.AsQueryable().OrderBy(x => (object) x.Id),
                DbSet.AsQueryable()
            },
            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Sort = new SortRule<FakeEntity, int>
                    {
                        SortOrder = SortOrder.Descending,
                        Expression = x => x.Id
                    }
                },
                DbSet.AsQueryable().OrderByDescending(x => (object) x.Id),
                DbSet.AsQueryable()
            },

            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Page = new PageRule
                    {
                        Index = 1,
                        Size = 5
                    }
                },
                DbSet.AsQueryable().Skip(5 * 1).Take(5),
                DbSet.AsQueryable()
            },

            new object[]
            {
                new QueryParameters<FakeEntity, int>
                {
                    Filter = new FilterRule<FakeEntity, int>
                    {
                        Expression = x => x.Id == 1
                    },
                    Sort = new SortRule<FakeEntity, int>
                    {
                        SortOrder = SortOrder.Descending,
                        Expression = x => x.Id
                    },
                    Page = new PageRule
                    {
                        Index = 1,
                        Size = 5
                    }
                },
                DbSet.AsQueryable().Where(x => x.Id == 1).OrderByDescending(x => (object) x.Id).Skip(5 * 1).Take(5),
                DbSet.AsQueryable().Where(x => x.Id == 1)
            }
        };

        private static void ValidateQueryParametersMemberData(IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            if (queryable == null) throw new ArgumentNullException(nameof(queryable));
            if (countQueryable == null) throw new ArgumentNullException(nameof(countQueryable));

            Assert.NotNull(queryable);
            Assert.NotNull(countQueryable);
        }

        #region ValidateQueryParametersMemberData

        [Fact]
        public void ValidateQueryParametersMemberData_QueryableIsNull_CatchException()
        {
            // Arrange && Act
            var exception = Assert.Throws<ArgumentNullException>(() => ValidateQueryParametersMemberData(null, new List<FakeEntity>().AsQueryable()));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'queryable')", exception.Message);
        }

        [Fact]
        public void ValidateQueryParametersMemberData_CountQueryableIsNull_CatchException()
        {
            // Arrange && Act
            var exception = Assert.Throws<ArgumentNullException>(() => ValidateQueryParametersMemberData(new List<FakeEntity>().AsQueryable(), null));

            // Assert
            Assert.Equal("Value cannot be null. (Parameter 'countQueryable')", exception.Message);
        }

        #endregion

        #region Query

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void Query_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act
            var expected = queryable.Expression.ToString();
            var actual = _fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).Expression.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void QueryTModel_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act
            var expected = _fixture.DataMapper.Map<FakeModel>(queryable).Expression.ToString();
            var actual = _fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery<FakeModel>(_fixture.DataMapper, queryParameters).Expression.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Get

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void Get_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).FirstOrDefault(), _fixture.UnitOfWork.FakeEntityRepository.Get(queryParameters));
        }

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void GetTModel_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            ObjectAssert.DeepEquals(_fixture.DataMapper.Map<FakeModel>(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters)).FirstOrDefault(), _fixture.UnitOfWork.FakeEntityRepository.Get<FakeModel>(_fixture.DataMapper, queryParameters));
        }

        #endregion

        #region GetList

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void GetList_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).ToList(), _fixture.UnitOfWork.FakeEntityRepository.GetList(queryParameters));
        }

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void GetListTModel_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            ObjectAssert.DeepEquals(_fixture.DataMapper.Map<FakeModel>(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters)).ToList(), _fixture.UnitOfWork.FakeEntityRepository.GetList<FakeModel>(_fixture.DataMapper, queryParameters));
        }

        #endregion

        #region PagedResultQuery

        [Fact]
        public void PagedResultQuery_QueryParametersIsNull_ArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _fixture.UnitOfWork.FakeEntityRepository.ProtectedPagedResultQuery(null));
            Assert.Equal("queryParameters", exception.ParamName);
            Assert.Equal("Value cannot be null. (Parameter 'queryParameters')", exception.Message);
        }

        [Fact]
        public void PagedResultQuery_QueryParametersPageIsNull_ArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _fixture.UnitOfWork.FakeEntityRepository.ProtectedPagedResultQuery(new QueryParameters<FakeEntity, int> { Page = null }));
            Assert.Equal("Page", exception.ParamName);
            Assert.Equal("Value cannot be null. (Parameter 'Page')", exception.Message);
        }

        [Fact]
        public void PagedResultQuery_QueryParametersPageIsNotValid_ArgumentException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _fixture.UnitOfWork.FakeEntityRepository.ProtectedPagedResultQuery(new QueryParameters<FakeEntity, int> { Page = new PageRule { Size = 0 } }));
            Assert.Equal("Query Parameters Page is not valid.", exception.Message);
        }

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void PagedResultQuery_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            if (queryParameters?.Page == null || queryParameters.Page.IsValid == false) return;

            // Arrange & Act
            var expected = queryable.Expression.ToString();
            var actual = _fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).Expression.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void PagedResultQueryTModel_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            if (queryParameters?.Page == null || queryParameters.Page.IsValid == false) return;

            // Arrange & Act
            var expected = _fixture.DataMapper.Map<FakeModel>(queryable).Expression.ToString();
            var actual = _fixture.UnitOfWork.FakeEntityRepository.ProtectedPagedResultQuery<FakeModel>(_fixture.DataMapper, queryParameters).Expression.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region GetPagedList

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void GetPagedList_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            if (queryParameters?.Page == null || queryParameters.Page.IsValid == false) return;

            // Arrange
            var pagedResultQuery = _fixture.UnitOfWork.FakeEntityRepository.ProtectedPagedResultQuery(queryParameters);

            var expectedPagedResult = new PagedResult<FakeEntity>
            {
                PageIndex = queryParameters.Page.Index,
                PageSize = queryParameters.Page.Size,
                Items = pagedResultQuery.ToList(),
                TotalCount = _fixture.UnitOfWork.FakeEntityRepository.Count(queryParameters)
            };

            // Act & Assert
            ObjectAssert.DeepEquals(expectedPagedResult, _fixture.UnitOfWork.FakeEntityRepository.GetPagedList(queryParameters));
        }

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void GetPagedListTModel_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            if (queryParameters?.Page == null || queryParameters.Page.IsValid == false) return;

            // Arrange
            var pagedResultQuery = _fixture.UnitOfWork.FakeEntityRepository.ProtectedPagedResultQuery<FakeModel>(_fixture.DataMapper, queryParameters);

            var expectedPagedResult = new PagedResult<FakeModel>
            {
                PageIndex = queryParameters.Page.Index,
                PageSize = queryParameters.Page.Size,
                Items = pagedResultQuery.ToList(),
                TotalCount = _fixture.UnitOfWork.FakeEntityRepository.Count(queryParameters)
            };

            // Act & Assert
            ObjectAssert.DeepEquals(expectedPagedResult, _fixture.UnitOfWork.FakeEntityRepository.GetPagedList<FakeModel>(_fixture.DataMapper, queryParameters));
        }

        #endregion

        #region Exists

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void ExistsQueryParameters_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).Any(), _fixture.UnitOfWork.FakeEntityRepository.Exists(queryParameters));
        }

        #endregion

        #region CountQuery

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void CountQuery_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act
            var expected = countQueryable.Expression.ToString();
            var actual = _fixture.UnitOfWork.FakeEntityRepository.ProtectedCountQuery(queryParameters).Expression.ToString();

            // Assert
            Assert.Equal(expected, actual);
        }

        #endregion

        #region Count

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public void Count_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedCountQuery(queryParameters).Count(), _fixture.UnitOfWork.FakeEntityRepository.Count(queryParameters));
        }

        #endregion

        #region Insert

        [Fact]
        public void Insert_NewEntity_Inserted()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Insert_NewEntity_Inserted));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            var repository = new Repository<FakeEntity, int>(fakeDbContext);

            var entity = new FakeEntity();

            // Act
            var insertedEntity = repository.Insert(entity);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(entity.Id, insertedEntity.Id);
        }

        [Fact]
        public void Insert_NewEntitiesList_Inserted()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Insert_NewEntity_Inserted));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            var repository = new Repository<FakeEntity, int>(fakeDbContext);

            var entities = new List<FakeEntity>
            {
                new FakeEntity(),
                new FakeEntity()
            };

            // Act
            repository.Insert(entities);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(fakeDbContext.Set<FakeEntity>().Count(), entities.Count);
        }

        #endregion

        #region Update

        [Fact]
        public void Update_ExistingEntity_Updated()
        {
            // Arrange
            const int id = 10;

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Update_ExistingEntity_Updated));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            var entity = fakeDbContext.Add(new FakeEntity { Id = id }).Entity;
            fakeDbContext.SaveChanges();

            var repository = new Repository<FakeEntity, int>(fakeDbContext);

            // Act
            entity = repository.Update(entity);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(id, entity.Id);
        }

        #endregion

        #region Delete

        [Fact]
        public void Delete_ExistingEntity_Deleted()
        {
            // Arrange
            const int id = 30;

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Delete_ExistingEntity_Deleted));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            var entity = fakeDbContext.Add(new FakeEntity { Id = id }).Entity;
            fakeDbContext.SaveChanges();

            var repository = new Repository<FakeEntity, int>(fakeDbContext);

            // Act
            repository.Delete(entity);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.False(fakeDbContext.Set<FakeEntity>().Any(x => x.Id == id));
        }

        [Fact]
        public void Delete_ExistingDetachedEntity_Deleted()
        {
            // Arrange
            const int id = 40;

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Delete_ExistingEntity_Deleted));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            var entity = fakeDbContext.Add(new FakeEntity { Id = id }).Entity;
            fakeDbContext.SaveChanges();

            fakeDbContext.Entry(entity).State = EntityState.Detached;

            var repository = new Repository<FakeEntity, int>(fakeDbContext);

            // Act
            repository.Delete(entity);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.False(fakeDbContext.Set<FakeEntity>().Any(x => x.Id == id));
        }

        [Fact]
        public void Delete_ExistingEntities_Deleted()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Delete_ExistingEntity_Deleted));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            fakeDbContext.AddRange(new List<FakeEntity>
            {
                new FakeEntity { Id = 1 },
                new FakeEntity { Id = 2 },
                new FakeEntity { Id = 3 }
            });
            fakeDbContext.SaveChanges();

            var repository = new Repository<FakeEntity, int>(fakeDbContext);

            // Act
            repository.Delete(new List<int> {2, 3});
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(1, fakeDbContext.Set<FakeEntity>().Count());
            Assert.Equal(1, fakeDbContext.Set<FakeEntity>().FirstOrDefault()?.Id);
        }

        #endregion
    }
}
