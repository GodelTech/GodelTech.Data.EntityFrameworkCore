using System;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void Inherit_IUnitOfWork()
        {
            // Arrange
            var mockDbContext = new Mock<DbContext>(MockBehavior.Strict);

            // Act
            var unitOfWork = new UnitOfWork(mockDbContext.Object);

            // Assert
            Assert.IsAssignableFrom<IUnitOfWork>(unitOfWork);
        }

        [Fact]
        public void Commit_InsertNewEntity_AffectedOneRow()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Commit_InsertNewEntity_AffectedOneRow));
            var unitOfWork = new FakeUnitOfWork(
                dbContext => new FakeRepository(dbContext),
                dbContextOptionsBuilder.Options,
                "dbo"
            );

            var entity = new FakeEntity();

            unitOfWork.FakeEntityRepository.Insert(entity);

            // Act & Assert
            Assert.Equal(1, unitOfWork.Commit());
        }

        [Fact]
        public void Commit_UpdateNonexistentEntity_DataStorageException()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Commit_UpdateNonexistentEntity_DataStorageException));
            var unitOfWork = new FakeUnitOfWork(
                dbContext => new FakeRepository(dbContext),
                dbContextOptionsBuilder.Options,
                "dbo"
            );

            var entity = new FakeEntity { Id = -1 };

            unitOfWork.FakeEntityRepository.Update(entity);

            // Act & Assert
            Assert.Throws<DataStorageException>(() => unitOfWork.Commit());
        }

        [Fact]
        public void Dispose_UnitOfWork_Success()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Dispose_UnitOfWork_Success));
            var fakeDbContext = new FakeDbContext(dbContextOptionsBuilder.Options, "dbo");

            UnitOfWork unitOfWork;

            using (unitOfWork = new UnitOfWork(fakeDbContext))
            {
                unitOfWork.Commit();
            }

            // Act
            unitOfWork.Dispose();

            // Assert
            Assert.Throws<ObjectDisposedException>(() => unitOfWork.Commit());
        }

        [Fact]
        public void Dispose_UnitOfWorkWithNullDbContext_Success()
        {
            // Arrange
            FakeUnitOfWork unitOfWork;

            using (unitOfWork = new FakeUnitOfWork())
            {
                Assert.NotNull(unitOfWork.FakeEntityRepository);
            }

            // Act
            unitOfWork.Dispose();

            // Assert
            var repository = unitOfWork.FakeEntityRepository;

            Assert.NotNull(repository);
        }

        [Fact]
        public void Dispose_FalseDispose_Success()
        {
            // Arrange
            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContext>().UseInMemoryDatabase(nameof(Dispose_FalseDispose_Success));
            var unitOfWork = new FakeUnitOfWork(
                dbContext => new FakeRepository(dbContext),
                dbContextOptionsBuilder.Options,
                "dbo"
            );

            // Act
            unitOfWork.DoNotDispose();

            // Assert
            Assert.NotNull(unitOfWork.FakeEntityRepository);
        }
    }
}
