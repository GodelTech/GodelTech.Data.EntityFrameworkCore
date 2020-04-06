using System;
using GodelTech.Data.EntityFrameworkCore;
using GodelTech.Demo.Data.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GodelTech.Demo.Data.Configurations
{
    public class PersonConfiguration : EntityTypeConfiguration<PersonEntity, int>
    {
        public PersonConfiguration(string schemaName)
            : base(schemaName)
        {

        }

        public override void Configure(EntityTypeBuilder<PersonEntity> builder)
        {
            builder.Property(x => x.Name).IsRequired();
        }
    }
}
