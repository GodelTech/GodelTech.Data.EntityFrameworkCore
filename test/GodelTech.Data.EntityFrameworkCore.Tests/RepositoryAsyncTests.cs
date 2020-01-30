using System.Linq;
using System.Threading.Tasks;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public partial class RepositoryTests
    {
        #region GetList

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task GetListAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).ToList(), await _fixture.UnitOfWork.FakeEntityRepository.GetListAsync(queryParameters));
        }

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task GetListTModelAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            //ObjectAssert.ValueEquals(_fixture.DataMapper.Map<FakeModel>(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters)).ToList(), await _fixture.UnitOfWork.FakeEntityRepository.GetListAsync<FakeModel>(_fixture.DataMapper, queryParameters));
        }

        #endregion

        #region GetPagedList

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task GetPagedListAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
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
            ObjectAssert.ValueEquals(expectedPagedResult, await _fixture.UnitOfWork.FakeEntityRepository.GetPagedListAsync(queryParameters));
        }

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task GetPagedListTModelAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
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
            ObjectAssert.ValueEquals(expectedPagedResult, await _fixture.UnitOfWork.FakeEntityRepository.GetPagedListAsync<FakeModel>(_fixture.DataMapper, queryParameters));
        }

        #endregion

        #region Exists

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task ExistsQueryParametersAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedQuery(queryParameters).Any(), await _fixture.UnitOfWork.FakeEntityRepository.ExistsAsync(queryParameters));
        }

        #endregion

        #region Count

        [Theory]
        [MemberData(nameof(QueryParametersMemberData))]
        public async Task CountAsync_Success(QueryParameters<FakeEntity, int> queryParameters, IQueryable<FakeEntity> queryable, IQueryable<FakeEntity> countQueryable)
        {
            ValidateQueryParametersMemberData(queryable, countQueryable);

            // Arrange & Act & Assert
            Assert.Equal(_fixture.UnitOfWork.FakeEntityRepository.ProtectedCountQuery(queryParameters).Count(), await _fixture.UnitOfWork.FakeEntityRepository.CountAsync(queryParameters));
        }

        #endregion

        #region Insert

        [Fact]
        public async Task InsertAsync_NewEntity_Inserted()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(InsertAsync_NewEntity_Inserted));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");
            var repository = new Repository<FakeEntity, int>(fakeDbContext);

            var entity = new FakeEntity();

            // Act
            var insertedEntity = await repository.InsertAsync(entity);
            fakeDbContext.SaveChanges();

            // Assert
            Assert.Equal(entity.Id, insertedEntity.Id);
        }

        #endregion
    }
}
