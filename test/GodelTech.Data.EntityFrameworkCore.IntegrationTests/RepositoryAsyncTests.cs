//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Aurochses.Xunit;
//using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
//using Microsoft.EntityFrameworkCore;
//using Xunit;

//namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests
//{
//    public partial class RepositoryTests
//    {
//        #region Get

//        [Theory]
//        [MemberData(nameof(QueryParametersMemberData))]
//        public async Task GetAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
//        {
//            ValidateQueryParametersMemberData(queryable, countQueryable);

//            // Arrange & Act & Assert
//            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).FirstOrDefault(), await _fixture.UnitOfWork.FakeEntityRepository.GetAsync(queryParameters));
//        }

//        [Theory]
//        [MemberData(nameof(QueryParametersMemberData))]
//        public async Task GetTModelAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
//        {
//            ValidateQueryParametersMemberData(queryable, countQueryable);

//            // Arrange & Act & Assert
//            ObjectAssert.DeepEquals(_fixture.DataMapper.Map<FakeModel>(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters)).FirstOrDefault(), await _fixture.UnitOfWork.FakeEntityRepository.GetAsync<FakeModel>(queryParameters));
//        }

//        #endregion

//        #region GetList

//        [Theory]
//        [MemberData(nameof(QueryParametersMemberData))]
//        public async Task GetListAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
//        {
//            ValidateQueryParametersMemberData(queryable, countQueryable);

//            // Arrange & Act & Assert
//            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).ToList(), await _fixture.UnitOfWork.FakeEntityRepository.GetListAsync(queryParameters));
//        }

//        [Theory]
//        [MemberData(nameof(QueryParametersMemberData))]
//        public async Task GetListTModelAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
//        {
//            ValidateQueryParametersMemberData(queryable, countQueryable);

//            // Arrange & Act & Assert
//            ObjectAssert.DeepEquals(_fixture.DataMapper.Map<FakeModel>(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters)).ToList(), await _fixture.UnitOfWork.FakeEntityRepository.GetListAsync<FakeModel>(queryParameters));
//        }

//        #endregion

//        #region GetPagedList

//        [Theory]
//        [MemberData(nameof(QueryParametersMemberData))]
//        public async Task GetPagedListAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
//        {
//            ValidateQueryParametersMemberData(queryable, countQueryable);

//            if (queryParameters?.Page == null || queryParameters.Page.IsValid == false) return;

//            // Arrange
//            var pagedResultQuery = _fixture.UnitOfWork.FakeEntityRepository.ProtectedPagedResultQuery(queryParameters);

//            var expectedPagedResult = new PagedResult<FakeEntity>
//            {
//                PageIndex = queryParameters.Page.Index,
//                PageSize = queryParameters.Page.Size,
//                Items = pagedResultQuery.ToList(),
//                TotalCount = _fixture.UnitOfWork.FakeEntityRepository.Count(queryParameters)
//            };

//            // Act & Assert
//            ObjectAssert.DeepEquals(expectedPagedResult, await _fixture.UnitOfWork.FakeEntityRepository.GetPagedListAsync(queryParameters));
//        }

//        [Theory]
//        [MemberData(nameof(QueryParametersMemberData))]
//        public async Task GetPagedListTModelAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
//        {
//            ValidateQueryParametersMemberData(queryable, countQueryable);

//            if (queryParameters?.Page == null || queryParameters.Page.IsValid == false) return;

//            // Arrange
//            var pagedResultQuery = _fixture.UnitOfWork.FakeEntityRepository.ProtectedPagedResultQuery<FakeModel>(queryParameters);

//            var expectedPagedResult = new PagedResult<FakeModel>
//            {
//                PageIndex = queryParameters.Page.Index,
//                PageSize = queryParameters.Page.Size,
//                Items = pagedResultQuery.ToList(),
//                TotalCount = _fixture.UnitOfWork.FakeEntityRepository.Count(queryParameters)
//            };

//            // Act & Assert
//            ObjectAssert.DeepEquals(expectedPagedResult, await _fixture.UnitOfWork.FakeEntityRepository.GetPagedListAsync<FakeModel>(queryParameters));
//        }

//        #endregion

//        #region Exists

//        [Theory]
//        [MemberData(nameof(QueryParametersMemberData))]
//        public async Task ExistsQueryParametersAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
//        {
//            ValidateQueryParametersMemberData(queryable, countQueryable);

//            // Arrange & Act & Assert
//            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).Any(), await _fixture.UnitOfWork.FakeEntityRepository.ExistsAsync(queryParameters));
//        }

//        #endregion

//        #region Count

//        [Theory]
//        [MemberData(nameof(QueryParametersMemberData))]
//        public async Task CountAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
//        {
//            ValidateQueryParametersMemberData(queryable, countQueryable);

//            // Arrange & Act & Assert
//            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedCountQuery(queryParameters).Count(), await _fixture.UnitOfWork.FakeEntityRepository.CountAsync(queryParameters));
//        }

//        #endregion

//        #region Insert

//        [Fact]
//        public async Task InsertAsync_NewEntity_Inserted()
//        {
//            // Arrange
//            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(InsertAsync_NewEntity_Inserted));
//            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
//            var repository = new Repository<FakeEntity, int>(fakeDbContext, _fixture.DataMapper);

//            var entity = new FakeEntity();

//            // Act
//            var insertedEntity = await repository.InsertAsync(entity);
//            await fakeDbContext.SaveChangesAsync();

//            // Assert
//            Assert.Equal(entity.Id, insertedEntity.Id);
//        }

//        [Fact]
//        public async Task InsertAsync_NewEntitiesList_Inserted()
//        {
//            // Arrange
//            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(InsertAsync_NewEntitiesList_Inserted));
//            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
//            var repository = new Repository<FakeEntity, int>(fakeDbContext, _fixture.DataMapper);

//            var entities = new List<FakeEntity>
//            {
//                new FakeEntity(),
//                new FakeEntity()
//            };

//            // Act
//            await repository.InsertAsync(entities);
//            await fakeDbContext.SaveChangesAsync();

//            // Assert
//            Assert.Equal(await fakeDbContext.Set<FakeEntity>().CountAsync(), entities.Count);
//        }

//        #endregion
//    }
//}