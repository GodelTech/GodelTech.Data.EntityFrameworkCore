using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeUnitOfWork : UnitOfWork
    {
        public FakeUnitOfWork(DbContext dbContext)
            : base(dbContext)
        {

        }

        public void ExposedRegisterRepository<TEntity, TKey>(IRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            RegisterRepository(repository);
        }

        public IRepository<TEntity, TKey> ExposedGetRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
        {
            return GetRepository<TEntity, TKey>();
        }

        public void ExposedDispose(bool disposing)
        {
            Dispose(disposing);
        }
    }
}