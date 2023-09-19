using System;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore.Tests.Fakes
{
    public class FakeUnitOfWork : UnitOfWork<DbContext>
    {
        private readonly Action<bool> _onDispose;
        private readonly Action<bool> _onAfterDispose;

        public FakeUnitOfWork(
            IDbContextFactory<DbContext> dbContextFactory,
            Action<bool> onDispose = null,
            Action<bool> onAfterDispose = null)
            : base(dbContextFactory)
        {
            _onDispose = onDispose;
            _onAfterDispose = onAfterDispose;
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

        public bool ExposedIsDisposed => IsDisposed;

        public void ExposedDispose(bool disposing)
        {
            Dispose(disposing);
        }

        protected override void Dispose(bool disposing)
        {
            _onDispose?.Invoke(disposing);

            base.Dispose(disposing);

            _onAfterDispose?.Invoke(IsDisposed);
        }
    }
}
