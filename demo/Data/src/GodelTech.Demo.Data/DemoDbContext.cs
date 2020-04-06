using GodelTech.Demo.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Demo.Data
{
    public class DemoDbContext : DbContext
    {
        private readonly string _schemaName;

        public DemoDbContext(DbContextOptions dbContextOptions, string schemaName)
            : base(dbContextOptions)
        {
            _schemaName = schemaName;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PersonConfiguration(_schemaName));
        }
    }
}
