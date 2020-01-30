using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public class EntityNoneDatabaseGeneratedIdentifierTests
    {
        private readonly EntityNoneDatabaseGeneratedIdentifier<int> _entity;

        public EntityNoneDatabaseGeneratedIdentifierTests()
        {
            _entity = new EntityNoneDatabaseGeneratedIdentifier<int>();
        }

        [Fact]
        public void Inherit_Entity()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<Entity<int>>(_entity);
        }

        [Theory]
        [InlineData(typeof(KeyAttribute), null, null)]
        [InlineData(typeof(DatabaseGeneratedAttribute), nameof(DatabaseGeneratedAttribute.DatabaseGeneratedOption), DatabaseGeneratedOption.None)]
        public void Id_Attribute_Defined(Type attributeType, string attributePropertyName, object attributePropertyValue)
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<EntityNoneDatabaseGeneratedIdentifier<int>>("Id", attributeType);

            if (attributePropertyName != null)
            {
                AttributeAssert.ValidateProperty(propertyInfo, attributeType, attributePropertyName, attributePropertyValue);
            }
        }

        [Fact]
        public void Id_Get_Success()
        {
            // Arrange & Act & Assert
            Assert.Equal(default(int), _entity.Id);
        }

        [Fact]
        public void Id_Set_Success()
        {
            // Arrange
            const int id = 1;

            // Act
            _entity.Id = id;

            // Assert
            Assert.Equal(id, _entity.Id);
        }
    }
}
