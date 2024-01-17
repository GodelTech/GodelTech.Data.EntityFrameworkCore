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
            ArgumentNullException.ThrowIfNull(modelBuilder);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FakeEntity<Guid>>();

            modelBuilder.Entity<FakeEntity<int>>();

            modelBuilder.Entity<FakeEntity<string>>();
        }
    }
}
