using System;
using System.Threading.Tasks;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public sealed class UnitOfWorkAsyncTests : IDisposable
    {
        private readonly Mock<DbContext> _mockDbContext;

        private readonly UnitOfWork<DbContext> _unitOfWork;

        public UnitOfWorkAsyncTests()
        {
            _mockDbContext = new Mock<DbContext>(MockBehavior.Strict);
            _mockDbContext
                .Setup(x => x.Dispose());

            var mockDbContextFactory = new Mock<IDbContextFactory<DbContext>>(MockBehavior.Strict);
            mockDbContextFactory
                .Setup(x => x.CreateDbContext())
                .Returns(_mockDbContext.Object);

            _unitOfWork = new FakeUnitOfWork(mockDbContextFactory.Object);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        [Fact]
        public async Task CommitAsync()
        {
            // Arrange
            const int expectedResult = 1;

            _mockDbContext
                .Setup(
                    x => x.SaveChangesAsync(default)
                )
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _unitOfWork.CommitAsync();

            // Assert
            _mockDbContext
                .Verify(
                    x => x.SaveChangesAsync(default),
                    Times.Once
                );

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task CommitAsync_WhenDbUpdateException_ThrowsDataStorageException()
        {
            // Arrange
            var expectedInnerException = new DbUpdateException("Test Message");

            _mockDbContext
                .Setup(
                    x => x.SaveChangesAsync(default)
                )
                .Throws(expectedInnerException);

            // Act
            var exception = await Assert.ThrowsAsync<DataStorageException>(
                () => _unitOfWork.CommitAsync()
            );

            // Assert
            _mockDbContext
                .Verify(
                    x => x.SaveChangesAsync(default),
                    Times.Once
                );

            Assert.Equal(expectedInnerException.Message, exception.Message);
            Assert.Equal(expectedInnerException, exception.InnerException);
        }
    }
}