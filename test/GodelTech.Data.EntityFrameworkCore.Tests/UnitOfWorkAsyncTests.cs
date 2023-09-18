using System;
using System.Threading;
using System.Threading.Tasks;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
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

            var mockDbContextFactory = new Mock<IDesignTimeDbContextFactory<DbContext>>(MockBehavior.Strict);
            mockDbContextFactory
                .Setup(x => x.CreateDbContext(Array.Empty<string>()))
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
            var cancellationToken = new CancellationToken();

            const int expectedResult = 1;

            _mockDbContext
                .Setup(
                    x => x.SaveChangesAsync(cancellationToken)
                )
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _unitOfWork.CommitAsync(cancellationToken);

            // Assert
            _mockDbContext
                .Verify(
                    x => x.SaveChangesAsync(cancellationToken),
                    Times.Once
                );

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public async Task CommitAsync_WhenDbUpdateException_ThrowsDataStorageException()
        {
            // Arrange
            var cancellationToken = new CancellationToken();

            var expectedInnerException = new DbUpdateException("Test Message");

            _mockDbContext
                .Setup(
                    x => x.SaveChangesAsync(cancellationToken)
                )
                .Throws(expectedInnerException);

            // Act
            var exception = await Assert.ThrowsAsync<DataStorageException>(
                () => _unitOfWork.CommitAsync(cancellationToken)
            );

            // Assert
            _mockDbContext
                .Verify(
                    x => x.SaveChangesAsync(cancellationToken),
                    Times.Once
                );

            Assert.Equal(expectedInnerException.Message, exception.Message);
            Assert.Equal(expectedInnerException, exception.InnerException);
        }
    }
}
