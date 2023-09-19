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

        private readonly FakeUnitOfWork _unitOfWork;

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

            _unitOfWork.ExposedRegisterRepository(repository);

            // Act
            var result = _unitOfWork.ExposedGetRepository<IEntity<TKey>, TKey>();

            // Assert
            Assert.NotNull(defaultKey);
            Assert.Equal(repository, result);
        }

        [Fact]
        public void Dispose_Success()
        {
            // Arrange
            var disposing = false;
            var disposeCalls = 0;

            bool isDisposedBeforeDispose;
            var isDisposedAfterDispose = false;

            WeakReference weak;

            void LocalFunction()
            {
                var unitOfWork = new FakeUnitOfWork(
                    _mockDbContextFactory.Object,
                    val =>
                    {
                        disposing = val;
                        disposeCalls++;
                    },
                    val => isDisposedAfterDispose = val
                );

                weak = new WeakReference(unitOfWork, true);

                isDisposedBeforeDispose = unitOfWork.ExposedIsDisposed;

                unitOfWork.Dispose();
            }

            // Act
            LocalFunction();

            // Arrange
            Assert.False(isDisposedBeforeDispose);

            GC.Collect(0, GCCollectionMode.Forced);
            GC.WaitForPendingFinalizers();

            _mockDbContext
                .Verify(
                    x => x.Dispose(),
                    Times.Once
                );

            Assert.True(disposing);
            Assert.Equal(1, disposeCalls);
            Assert.True(isDisposedAfterDispose);

            Assert.False(weak.IsAlive);
        }

        [Fact]
        public void Dispose_WhenIsDisposed()
        {
            // Arrange
            _unitOfWork.Dispose();

            // Act
            _unitOfWork.ExposedDispose(true);

            // Assert
            _mockDbContext
                .Verify(
                    x => x.Dispose(),
                    Times.Once
                );

            Assert.True(_unitOfWork.ExposedIsDisposed);
        }

        [Fact]
        public void Dispose_WithFalse()
        {
            // Arrange & Act
            _unitOfWork.ExposedDispose(false);

            // Assert
            _mockDbContext
                .Verify(
                    x => x.Dispose(),
                    Times.Never
                );

            Assert.False(_unitOfWork.ExposedIsDisposed);
        }

        [Fact]
        public void Dispose_WhenDbContextIsNull()
        {
            // Arrange
            _mockDbContextFactory
                .Setup(x => x.CreateDbContext())
                .Returns(() => null);

            // Act
            _unitOfWork.ExposedDispose(true);

            // Assert
            Assert.True(_unitOfWork.ExposedIsDisposed);
        }
    }
}
