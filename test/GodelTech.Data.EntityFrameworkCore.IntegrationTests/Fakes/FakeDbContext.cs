using System;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes
{
    public class FakeDbContext : DbContextBase
    {
        public FakeDbContext(DbContextOptions options, string schemaName)
            : base(options, schemaName)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FakeEntity<Guid>>();

            modelBuilder.Entity<FakeEntity<int>>();

            modelBuilder.Entity<FakeEntity<string>>();
        }
    }
}
