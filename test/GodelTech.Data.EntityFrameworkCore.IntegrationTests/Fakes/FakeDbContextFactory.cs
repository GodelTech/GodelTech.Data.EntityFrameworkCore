using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes
{
    public class FakeDbContextFactory : IDesignTimeDbContextFactory<FakeDbContext>
    {
        private readonly DbContextOptions _dbContextOptions;

        public FakeDbContextFactory(DbContextOptions dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public FakeDbContext CreateDbContext(string[] args)
        {
            return new FakeDbContext(_dbContextOptions, "dbo");
        }
    }
}
