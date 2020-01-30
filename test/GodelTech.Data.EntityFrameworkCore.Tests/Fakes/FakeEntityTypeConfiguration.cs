using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    [ExcludeFromCodeCoverage]
    public class FakeEntityTypeConfiguration
        : EntityTypeConfiguration<Entity<int>, int>
    {
        public FakeEntityTypeConfiguration(string schemaName)
            : base(schemaName)
        {

        }

        public override void Configure(EntityTypeBuilder<Entity<int>> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("FakeEntity", SchemaName);
        }
    }
}
