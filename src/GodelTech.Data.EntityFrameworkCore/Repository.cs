using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore
{
    /// <summary>
    /// Repository for data layer
    /// </summary>
    /// <typeparam name="TEntity">The type of the T entity.</typeparam>
    /// <typeparam name="TType">The type of the T type.</typeparam>
    public partial class Repository<TEntity, TType> : IRepository<TEntity, TType>
        where TEntity : class, IEntity<TType>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{TEntity, TType}"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="dataMapper">The data mapper.</param>
        public Repository(DbContext dbContext, IDataMapper dataMapper)
        {
            DbContext = dbContext;
            DataMapper = dataMapper;
        }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>The database context.</value>
        protected DbContext DbContext { get; }

        /// <summary>
        /// Gets the database set.
        /// </summary>
        /// <value>The database set.</value>
        protected DbSet<TEntity> DbSet => DbContext.Set<TEntity>();

        /// <summary>
        /// Gets the data mapper.
        /// </summary>
        /// <value>The data mapper.</value>
        protected IDataMapper DataMapper { get; }

        /// <summary>
        /// Gets query of type T for repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IQueryable{TEntity}</cref>.</returns>
        protected virtual IQueryable<TEntity> Query(QueryParameters<TEntity, TType> queryParameters = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (queryParameters == null) return query;

            if (queryParameters.Filter?.Expression != null)
            {
                query = query.Where(queryParameters.Filter.Expression);
            }

            if (queryParameters.Sort != null && queryParameters.Sort.IsValid)
            {
                query = queryParameters.Sort.SortOrder == SortOrder.Descending
                    ? query.OrderByDescending(queryParameters.Sort.Expression)
                    : query.OrderBy(queryParameters.Sort.Expression);
            }

            if (queryParameters.Page != null && queryParameters.Page.IsValid)
            {
                query = query
                    .Skip(queryParameters.Page.Size * queryParameters.Page.Index)
                    .Take(queryParameters.Page.Size);
            }

            return query;
        }

        /// <summary>
        /// Gets query of type T for repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IQueryable{TEntity}</cref>.</returns>
        protected virtual IQueryable<TModel> Query<TModel>(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return DataMapper.Map<TModel>(
                Query(queryParameters)
            );
        }

        /// <summary>
        /// Gets query of type T for repository that satisfies a query parameters for paged result.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IQueryable{TEntity}</cref>.</returns>
        protected virtual IQueryable<TEntity> PagedResultQuery(QueryParameters<TEntity, TType> queryParameters)
        {
            if (queryParameters == null) throw new ArgumentNullException(nameof(queryParameters));

            if (queryParameters.Page == null) throw new ArgumentException("Page can't be null.", nameof(queryParameters));

            if (!queryParameters.Page.IsValid) throw new ArgumentException("Page is not valid.", nameof(queryParameters));

            return Query(queryParameters);
        }

        /// <summary>
        /// Gets query of type T for repository that satisfies a query parameters for paged result.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IQueryable{TEntity}</cref>.</returns>
        protected IQueryable<TModel> PagedResultQuery<TModel>(QueryParameters<TEntity, TType> queryParameters)
        {
            return DataMapper.Map<TModel>(
                PagedResultQuery(queryParameters)
            );
        }

        /// <summary>
        /// Gets query of type T for repository that satisfies a query parameters for count.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IQueryable{TEntity}</cref>.</returns>
        protected virtual IQueryable<TEntity> CountQuery(QueryParameters<TEntity, TType> queryParameters = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (queryParameters == null) return query;

            if (queryParameters.Filter?.Expression != null)
            {
                query = query.Where(queryParameters.Filter.Expression);
            }

            return query;
        }

        /// <summary>
        /// Marks entity as modified.
        /// </summary>
        /// <param name="entity">The entity.</param>
        protected void MarkAsModified(TEntity entity)
        {
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        /// <summary>
        /// Determines whether the specified entity is detached.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><c>true</c> if the specified entity is detached; otherwise, <c>false</c>.</returns>
        protected bool IsDetached(TEntity entity)
        {
            return DbContext.Entry(entity).State == EntityState.Detached;
        }
    }
}