using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeEntityTypeConfiguration<TEntity, TKey> : EntityTypeConfiguration<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
    {
        public FakeEntityTypeConfiguration(string schemaName)
            : base(schemaName)
        {

        }

        public override void Configure(EntityTypeBuilder<TEntity> builder)
        {

        }
    }
}