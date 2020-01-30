using GodelTech.Data.EntityFrameworkCore.Tests.Fakes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Moq;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public class EntityTypeConfigurationTests
    {
        [Fact]
        public void SchemaName_Get_Success()
        {
            // Arrange
            const string schemaName = "dbo";

            var entityTypeConfiguration = new FakeEntityTypeConfiguration(schemaName);

            // Act & Assert
            Assert.Equal(schemaName, entityTypeConfiguration.SchemaName);
        }

        [Fact]
        public void Map_EntityTypeBuilder_Success()
        {
            // Arrange
            var modelBuilder = new ModelBuilder(new ConventionSet());
            var entityTypeBuilder = modelBuilder.Entity<Entity<int>>();

            var mockEntityTypeConfiguration = new Mock<EntityTypeConfiguration<Entity<int>, int>>(MockBehavior.Strict, "dbo");
            mockEntityTypeConfiguration.Setup(m => m.Configure(entityTypeBuilder)).Verifiable();

            // Act
            mockEntityTypeConfiguration.Object.Configure(entityTypeBuilder);

            // Assert
            mockEntityTypeConfiguration.Verify();
        }
    }
}
