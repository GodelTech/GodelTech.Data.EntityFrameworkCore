using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Aurochses.Xunit;
using Xunit;

namespace GodelTech.Data.EntityFrameworkCore.Tests
{
    public class EntityTests
    {
        private readonly Entity<int> _entity;

        public EntityTests()
        {
            _entity = new Entity<int>();
        }

        [Fact]
        public void Inherit_IEntity()
        {
            // Arrange & Act & Assert
            Assert.IsAssignableFrom<IEntity<int>>(_entity);
        }

        [Theory]
        [InlineData(typeof(KeyAttribute), null, null)]
        [InlineData(typeof(DatabaseGeneratedAttribute), nameof(DatabaseGeneratedAttribute.DatabaseGeneratedOption), DatabaseGeneratedOption.Identity)]
        public void Id_Attribute_Defined(Type attributeType, string attributePropertyName, object attributePropertyValue)
        {
            // Arrange & Act & Assert
            var propertyInfo = TypeAssert.PropertyHasAttribute<Entity<int>>("Id", attributeType);

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

        [Fact]
        public void Equals_EntityAndNull_False()
        {
            // Arrange & Act & Assert
            Assert.False(_entity.Equals(null));
        }

        [Fact]
        public void Equals_TwoEqualEntities_True()
        {
            // Arrange
            const int id = 1;

            var firstEntity = new Entity<int> { Id = id };
            var secondEntity = new Entity<int> { Id = id };

            // Act & Assert
            Assert.True(firstEntity.Equals(firstEntity, secondEntity));
        }

        [Fact]
        public void Equals_EntityAndNullObject_False()
        {
            // Arrange & Act & Assert
            Assert.False(_entity.Equals(new object()));
        }

        [Fact]
        public void GetHashCode_TwoEqualEntities_Equal()
        {
            // Arrange
            const int id = 1;

            var firstEntity = new Entity<int> { Id = id };
            var secondEntity = new Entity<int> { Id = id };

            // Arrange & Act & Assert
            Assert.Equal(firstEntity.GetHashCode(firstEntity), secondEntity.GetHashCode(secondEntity));
        }

        [Fact]
        public void GetHashCode_TwoNotEqualEntities_NotEqual()
        {
            // Arrange
            var firstEntity = new Entity<int> { Id = 1 };
            var secondEntity = new Entity<int> { Id = 2 };

            // Arrange & Act & Assert
            Assert.NotEqual(firstEntity.GetHashCode(firstEntity), secondEntity.GetHashCode(secondEntity));
        }
    }
}
