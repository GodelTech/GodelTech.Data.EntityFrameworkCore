using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public sealed class UnitOfWorkTests : IDisposable
    {
        private readonly Mock<DbContext> _mockDbContext;

        private readonly Mock<IDbContextFactory<DbContext>> _mockDbContextFactory;

        private readonly UnitOfWork<DbContext> _unitOfWork;

        public UnitOfWorkTests()
        {
            _mockDbContext = new Mock<DbContext>(MockBehavior.Strict);
            _mockDbContext
                .Setup(x => x.Dispose());

            _mockDbContextFactory = new Mock<IDbContextFactory<DbContext>>(MockBehavior.Strict);
            _mockDbContextFactory
                .Setup(x => x.CreateDbContext())
                .Returns(_mockDbContext.Object);

            _unitOfWork = new FakeUnitOfWork(_mockDbContextFactory.Object);
        }

        public void Dispose()
        {
            _unitOfWork.Dispose();
        }

        [Fact]
        public void Constructor_ThrowsArgumentNullException()
        {
            // Arrange & Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(
                () => new FakeUnitOfWork(null)
            );
            Assert.Equal("dbContextFactory", exception.ParamName);
        }

        // https://blog.ingeniumsoftware.dev/unit-testing-finalizers-in-csharp/
        [Fact]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
        public void Finalizer_DisposeWithFalse()
        {
            // Arrange
            WeakReference<FakeUnitOfWork> weak = null;
            Action dispose = () =>
            {
                // This will go out of scope after dispose() is executed
                var unitOfWork = new FakeUnitOfWork(_mockDbContextFactory.Object);

                weak = new WeakReference<FakeUnitOfWork>(unitOfWork, true);
            };

            // Act
            dispose();
            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            // Assert
            _mockDbContext
                .Verify(
                    x => x.Dispose(),
                    Times.Never
                );
        }

        [Fact]
        public void Commit()
        {
            // Arrange
            const int expectedResult = 1;

            _mockDbContext
                .Setup(
                    x => x.SaveChanges()
                )
                .Returns(expectedResult);

            // Act
            var result = _unitOfWork.Commit();

            // Assert
            _mockDbContext
                .Verify(
                    x => x.SaveChanges(),
                    Times.Once
                );

            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Commit_WhenDbUpdateException_ThrowsDataStorageException()
        {
            // Arrange
            var expectedInnerException = new DbUpdateException("Test Message");

            _mockDbContext
                .Setup(
                    x => x.SaveChanges()
                )
                .Throws(expectedInnerException);

            // Act
            var exception = Assert.Throws<DataStorageException>(
                () => _unitOfWork.Commit()
            );

            // Assert
            _mockDbContext
                .Verify(
                    x => x.SaveChanges(),
                    Times.Once
                );

            Assert.Equal(expectedInnerException.Message, exception.Message);
            Assert.Equal(expectedInnerException, exception.InnerException);
        }

        public static IEnumerable<object[]> TypesMemberData =>
            new Collection<object[]>
            {
                // Guid
                new object[]
                {
                    default(Guid)
                },
                // int
                new object[]
                {
                    default(int)
                },
                // string
                new object[]
                {
                    string.Empty
                }
            };

        [Theory]
        [MemberData(nameof(TypesMemberData))]
        public void Repositories_Success<TKey>(TKey defaultKey)
        {
            // Arrange
            var mockDataMapper = new Mock<IDataMapper>(MockBehavior.Strict);

            var repository = new Repository<IEntity<TKey>, TKey>(
                _mockDbContext.Object,
                mockDataMapper.Object
            );

            var fakeUnitOfWork = (FakeUnitOfWork) _unitOfWork;

            // Act
            fakeUnitOfWork.ExposedRegisterRepository(repository);
            var result = fakeUnitOfWork.ExposedGetRepository<IEntity<TKey>, TKey>();

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(repository, result);
        }

        [Fact]
        public void Dispose_WithFalse()
        {
            // Arrange
            var fakeUnitOfWork = (FakeUnitOfWork) _unitOfWork;

            // Act
            fakeUnitOfWork.ExposedDispose(false);

            // Assert
            _mockDbContext
                .Verify(
                    x => x.Dispose(),
                    Times.Never
                );
        }

        [Fact]
        public void Dispose_WhenIsDisposed()
        {
            // Arrange
            var fakeUnitOfWork = (FakeUnitOfWork) _unitOfWork;

            // Act
            fakeUnitOfWork.Dispose();

            fakeUnitOfWork.ExposedDispose(true);

            // Assert
            _mockDbContext
                .Verify(
                    x => x.Dispose(),
                    Times.Once
                );
        }

        [Fact]
        public void Dispose_WhenDbContextIsNull()
        {
            // Arrange
            var mockDbContextFactory = new Mock<IDbContextFactory<DbContext>>(MockBehavior.Strict);
            mockDbContextFactory
                .Setup(x => x.CreateDbContext())
                .Returns(() => null);

            using var fakeUnitOfWork = new FakeUnitOfWork(mockDbContextFactory.Object);

            // Act
            fakeUnitOfWork.ExposedDispose(true);

            // Assert
            _mockDbContext
                .Verify(
                    x => x.Dispose(),
                    Times.Never
                );
        }
    }
}