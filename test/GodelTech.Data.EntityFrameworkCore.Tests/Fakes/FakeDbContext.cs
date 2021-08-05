using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeDbContext : DbContextBase
    {
        public FakeDbContext(DbContextOptions options, string schemaName)
            : base(options, schemaName)
        {

        }
    }
}