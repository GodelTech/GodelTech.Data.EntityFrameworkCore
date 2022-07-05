using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.IntegrationTests.Fakes
{
    public class FakeDbContextFactory : IDbContextFactory<FakeDbContext>
    {
        private readonly DbContextOptions _dbContextOptions;

        public FakeDbContextFactory(DbContextOptions dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public FakeDbContext CreateDbContext()
        {
            return new FakeDbContext(_dbContextOptions, "dbo");
        }
    }
}