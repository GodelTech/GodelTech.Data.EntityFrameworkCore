using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore
{
    /// <summary>
    /// UnitOfWork for data layer.
    /// </summary>
    /// <seealso cref="IUnitOfWork" />
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DbContext _dbContext;
        private readonly IDictionary<Type, object> _repositories;
        private bool _isDisposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public UnitOfWork(DbContext dbContext)
        {
            _repositories = new Dictionary<Type, object>();
            _dbContext = dbContext;
        }

        ~UnitOfWork()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>The database context.</value>
        protected DbContext DbContext => _dbContext;

        /// <summary>
        /// Dispose object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
            DisposeDbContext();
            _isDisposed = true;
        }

        /// <summary>
        /// Disposes the database context.
        /// </summary>
        private void DisposeDbContext()
        {
            _dbContext?.Dispose();
        }

        #endregion

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
                cnt = _dbContext.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                throw new DataStorageException(exception);
            }

            return cnt;
        }

        /// <summary>
        /// Asynchronously commits all changes on the DB.
        /// </summary>
        /// <returns>Number of rows affected.</returns>
        /// <exception cref="DataStorageException"></exception>
        public virtual async Task<int> CommitAsync()
        {
            int cnt;

            try
            {
                cnt = await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException exception)
            {
                throw new DataStorageException(exception);
            }

            return cnt;
        }

        /// <summary>
        /// Registers repository instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <typeparam name="TType">The type of the T type.</typeparam>
        /// <param name="repository">The repository.</param>
        protected void RegisterRepository<TEntity, TType>(IRepository<TEntity, TType> repository)
            where TEntity : class, IEntity<TType>
        {
            _repositories[typeof(TEntity)] = repository;
        }

        /// <summary>
        /// Gets the repository for specified entity type.
        /// </summary>
        /// <typeparam name="TEntity">The type of the T entity.</typeparam>
        /// <typeparam name="TType">The type of the T type.</typeparam>
        /// <returns>IRepository{TEntity, TType}.</returns>
        protected virtual IRepository<TEntity, TType> GetRepository<TEntity, TType>()
            where TEntity : class, IEntity<TType>
        {
            return (IRepository<TEntity, TType>)_repositories[typeof(TEntity)];
        }
    }
}
