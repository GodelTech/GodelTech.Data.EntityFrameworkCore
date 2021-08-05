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
            var mockData = entities.AsQueryable().BuildMock();

            var mockDbSet = mockData.Object.BuildMockDbSet();

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