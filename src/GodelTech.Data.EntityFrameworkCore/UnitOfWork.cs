using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

[assembly: CLSCompliant(false)]
namespace GodelTech.Data.EntityFrameworkCore
{
    /// <summary>
    /// UnitOfWork for data layer.
    /// </summary>
    /// <seealso cref="IUnitOfWork" />
    public abstract class UnitOfWork<TDbContext> : IUnitOfWork, IChangeTracker
        where TDbContext : DbContext
    {
        private readonly IDictionary<Type, object> _repositories = new Dictionary<Type, object>();
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork{TDbContext}"/> class.
        /// </summary>
        /// <param name="dbContextFactory">The database context factory.</param>
        protected UnitOfWork(IDbContextFactory<TDbContext> dbContextFactory)
        {
            if (dbContextFactory == null) throw new ArgumentNullException(nameof(dbContextFactory));

            DbContext = dbContextFactory.CreateDbContext();
        }

        /// <summary>
        /// Destructor
        /// </summary>
        ~UnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>The database context.</value>
        protected TDbContext DbContext { get; }

        /// <summary>
        /// Commits all changes on the DB.
        /// </summary>
        /// <returns>Number of rows affected.</returns>
        /// <exception cref="DataStorageException"></exception>
        public virtual int Commit()
        {
            int cnt;

            try
            {
                cnt = DbContext.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                throw new DataStorageException(exception.Message, exception);
            }

            return cnt;
        }

        /// <summary>
        /// Asynchronously commits all changes on the DB.
        /// </summary>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>Number of rows affected.</returns>
        /// <exception cref="DataStorageException"></exception>
        public virtual async Task<int> CommitAsync(CancellationToken cancellationToken = default)
        {
            int cnt;

            try
            {
                cnt = await DbContext.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException exception)
            {
                throw new DataStorageException(exception.Message, exception);
            }

            return cnt;
        }

        /// <summary>
        /// Stops tracking all currently tracked entities.
        /// </summary>
        public void ClearChangeTracker()
        {
            DbContext.ChangeTracker.Clear();
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Registers repository instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <typeparam name="TKey">The type of the T key.</typeparam>
        /// <param name="repository">The repository.</param>
        protected void RegisterRepository<TEntity, TKey>(IRepository<TEntity, TKey> repository)
            where TEntity : class, IEntity<TKey>
        {
            _repositories[typeof(TEntity)] = repository;
        }

        /// <summary>
        /// Gets the repository for specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <typeparam name="TKey">The type of the T key.</typeparam>
        /// <returns>IRepository{TEntity, TKey}.</returns>
        protected virtual IRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : class, IEntity<TKey>
        {
            return GetRepository<IRepository<TEntity, TKey>, TEntity, TKey>();
        }

        /// <summary>
        /// Gets the repository for specified entity type.
        /// </summary>
        /// <typeparam name="TRepository">The type of the T repository.</typeparam>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <typeparam name="TKey">The type of the T key.</typeparam>
        /// <returns>TRepository.</returns>
        protected virtual TRepository GetRepository<TRepository, TEntity, TKey>()
            where TRepository : IRepository<TEntity, TKey>
            where TEntity : class, IEntity<TKey>
        {
            return (TRepository) _repositories[typeof(TEntity)];
        }

        #region Dispose

        /// <summary>
        /// If disposing equals true, the method has been called directly
        /// or indirectly by a user's code. Managed and unmanaged resources
        /// are disposed.
        /// If disposing equals false, the method has been called by the
        /// runtime from inside the finalizer: only unmanaged resources can be disposed.
        /// </summary>
        /// <param name="disposing">
        /// Indicates, whether method has been
        /// called directly by user code from Dispose() or from Finalizer.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                // unmanaged resources would be cleaned up here.
                return;
            }

            if (_isDisposed)
            {
                // no need to dispose twice.
                return;
            }

            // free managed resources 
            DbContext?.Dispose();
            _isDisposed = true;
        }

        #endregion
    }
}
