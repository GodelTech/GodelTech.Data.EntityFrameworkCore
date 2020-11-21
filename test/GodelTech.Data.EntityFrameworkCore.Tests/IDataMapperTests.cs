using System.Collections.Generic;
using System.Linq;
using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Moq;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    // ReSharper disable once InconsistentNaming
    public class IDataMapperTests
    {
        private readonly Mock<IDataMapper> _mockDataMapper;

        public IDataMapperTests()
        {
            _mockDataMapper = new Mock<IDataMapper>(MockBehavior.Strict);
        }

        [Fact]
        public void Map_IQueryable_ReturnQueryableOfDestination()
        {
            // Arrange
            var queryable = new List<FakeEntity>().AsQueryable();
            var expected = new List<FakeModel>().AsQueryable();

            _mockDataMapper.Setup(m => m.Map<FakeModel>(queryable)).Returns(expected);

            // Act & Assert
            Assert.Equal(expected, _mockDataMapper.Object.Map<FakeModel>(queryable));
        }
    }
}