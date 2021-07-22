using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public partial class RepositoryAsyncTests
    {
        private readonly Mock<DbContext> _mockDbContext;
        private readonly Mock<IDataMapper> _mockDataMapper;

        public RepositoryAsyncTests()
        {
            _mockDbContext = new Mock<DbContext>(MockBehavior.Strict);
            _mockDataMapper = new Mock<IDataMapper>(MockBehavior.Strict);
        }

        public Repository<TEntity, TKey> GetRepository<TEntity, TKey>(ICollection<TEntity> entities)
            where TEntity : class, IEntity<TKey>
        {
            var mockQueryable = entities.AsQueryable().BuildMock();

            var mockDbSet = new Mock<DbSet<TEntity>>(MockBehavior.Strict);

            mockDbSet.As<IQueryable<TEntity>>().Setup(x => x.Provider).Returns(mockQueryable.Object.Provider);
            mockDbSet.As<IQueryable<TEntity>>().Setup(x => x.Expression).Returns(mockQueryable.Object.Expression);
            mockDbSet.As<IQueryable<TEntity>>().Setup(x => x.ElementType).Returns(mockQueryable.Object.ElementType);
            mockDbSet.As<IQueryable<TEntity>>().Setup(x => x.GetEnumerator()).Returns(mockQueryable.Object.GetEnumerator());

            _mockDbContext
                .Setup(x => x.Set<TEntity>())
                .Returns(() => mockDbSet.Object);

            return new Repository<TEntity, TKey>(
                _mockDbContext.Object,
                _mockDataMapper.Object
            );
        }
    }
}