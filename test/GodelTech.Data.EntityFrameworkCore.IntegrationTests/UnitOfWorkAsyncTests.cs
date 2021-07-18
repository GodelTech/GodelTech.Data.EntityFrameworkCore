//using System.Threading.Tasks;
//using GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes;
//using Microsoft.EntityFrameworkCore;
//using Xunit;

//namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests
//{
//    public class UnitOfWorkAsyncTests
//    {
//        [Fact]
//        public async Task CommitAsync_InsertNewEntity_AffectedOneRow()
//        {
//            // Arrange
//            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(CommitAsync_InsertNewEntity_AffectedOneRow));
//            var dataMapper = new FakeDataMapper();
//            var unitOfWork = new FakeUnitOfWork(
//                dbContext => new FakeRepository(dbContext, dataMapper),
//                dbContextOptionsBuilder.Options,
//                "dbo"
//            );

//            var entity = new FakeEntity();

//            await unitOfWork.FakeEntityRepository.InsertAsync(entity);

//            // Act & Assert
//            Assert.Equal(1, await unitOfWork.CommitAsync());
//        }

//        [Fact]
//        public async Task CommitAsync_UpdateNonexistentEntity_DataStorageException()
//        {
//            // Arrange
//            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(CommitAsync_UpdateNonexistentEntity_DataStorageException));
//            var dataMapper = new FakeDataMapper();
//            var unitOfWork = new FakeUnitOfWork(
//                dbContext => new FakeRepository(dbContext, dataMapper),
//                dbContextOptionsBuilder.Options,
//                "dbo"
//            );

//            var entity = new FakeEntity { Id = -1 };

//            unitOfWork.FakeEntityRepository.Update(entity);

//            // Act & Assert
//            await Assert.ThrowsAsync<DataStorageException>(async () => await unitOfWork.CommitAsync());
//        }
//    }
//}