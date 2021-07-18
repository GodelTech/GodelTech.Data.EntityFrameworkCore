//using Microsoft.EntityFrameworkCore;
//using Xunit;

//namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests
//{
//    public class DbContextBaseTests
//    {
//        [Fact]
//        public void Inherit_DbContext()
//        {
//            // Arrange
//            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContextBase>();
//            var dbContext = new DbContextBase(dbContextOptionsBuilder.Options, "dbo");

//            // Act & Assert
//            Assert.IsAssignableFrom<DbContext>(dbContext);
//        }

//        [Fact]
//        public void SchemaName_Get_Success()
//        {
//            // Arrange
//            const string schemaName = "dbo";

//            var dbContextOptionsBuilder = new DbContextOptionsBuilder<DbContextBase>();
//            var dbContext = new DbContextBase(dbContextOptionsBuilder.Options, schemaName);

//            // Act & Assert
//            Assert.Equal(schemaName, dbContext.SchemaName);
//        }
//    }
//}