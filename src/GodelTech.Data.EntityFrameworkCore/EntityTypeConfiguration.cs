using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodelTech.Data.EntityFrameworkCore
{
    /// <summary>
    /// EntityTypeConfiguration.
    /// </summary>
    /// <typeparam name="TEntity">The type of the T entity.</typeparam>
    /// <typeparam name="TKey">The type of the T key.</typeparam>
    public abstract class EntityTypeConfiguration<TEntity, TKey> : IEntityTypeConfiguration<TEntity>
        where TEntity : class, IEntity<TKey>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EntityTypeConfiguration{TEntity, TKey}"/> class.
        /// </summary>
        /// <param name="schemaName">Name of the schema.</param>
        protected EntityTypeConfiguration(string schemaName)
        {
            SchemaName = schemaName;
        }

        /// <summary>
        /// Gets the name of the schema.
        /// </summary>
        /// <value>The name of the schema.</value>
        public string SchemaName { get; }

        /// <summary>
        /// Configures the entity of type <typeparamref name="TEntity" />.
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity type.</param>
        public abstract void Configure(EntityTypeBuilder<TEntity> builder);
    }
}