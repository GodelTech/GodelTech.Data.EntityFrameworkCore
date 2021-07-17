using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GodelTech.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore
{
    /// <summary>
    /// Repository for data layer
    /// </summary>
    /// <typeparam name="TEntity">The type of the T entity.</typeparam>
    /// <typeparam name="TType">The type of the T type.</typeparam>
    public class Repository<TEntity, TType> : IRepository<TEntity, TType>
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

            if (queryParameters.Sort?.Expression != null)
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
            return DataMapper.Map<TModel>(Query(queryParameters));
        }

        /// <summary>
        /// Gets entity of type T from repository that satisfies a query parameters.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>TEntity</cref>.</returns>
        public virtual TEntity Get(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return Query(queryParameters).FirstOrDefault();
        }

        /// <summary>
        /// Gets model of type T from repository that satisfies a query parameters.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>TModel</cref></returns>
        public virtual TModel Get<TModel>(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return Query<TModel>(queryParameters).FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously gets entity of type T from repository that satisfies a query parameters.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{TEntity}</cref>.</returns>
        public virtual async Task<TEntity> GetAsync(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await Query(queryParameters).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Asynchronously gets model of type T from repository that satisfies a query parameters.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{TModel}</cref>.</returns>
        public virtual async Task<TModel> GetAsync<TModel>(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await Query<TModel>(queryParameters).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Gets entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IList{TEntity}</cref>.</returns>
        public virtual IList<TEntity> GetList(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return Query(queryParameters).ToList();
        }

        /// <summary>
        /// Gets models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IList{TModel}</cref>.</returns>
        public virtual IList<TModel> GetList<TModel>(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return Query<TModel>(queryParameters).ToList();
        }

        /// <summary>
        /// Asynchronously gets entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{IList{TModel}}</cref>.</returns>
        public virtual async Task<IList<TEntity>> GetListAsync(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await Query(queryParameters).ToListAsync();
        }

        /// <summary>
        /// Asynchronously gets models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{IList{TModel}}</cref>.</returns>
        public virtual async Task<IList<TModel>> GetListAsync<TModel>(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await Query<TModel>(queryParameters).ToListAsync();
        }

        /// <summary>
        /// Gets query of type T for repository that satisfies a query parameters for paged result.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IQueryable{TEntity}</cref>.</returns>
        protected virtual IQueryable<TEntity> PagedResultQuery(QueryParameters<TEntity, TType> queryParameters)
        {
            if (queryParameters == null)
                throw new ArgumentNullException(nameof(queryParameters), "Query Parameters can't be null.");

            if (queryParameters.Page == null)
                throw new ArgumentNullException(string.Empty, "Query Parameters Page can't be null.");

            if (!queryParameters.Page.IsValid)
                throw new ArgumentException("Query Parameters Page is not valid.");

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
            return DataMapper.Map<TModel>(PagedResultQuery(queryParameters));
        }

        /// <summary>
        /// Gets paged list of entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>PagedResult{TEntity}</cref>.</returns>
        public virtual PagedResult<TEntity> GetPagedList(QueryParameters<TEntity, TType> queryParameters)
        {
            var items = PagedResultQuery(queryParameters).ToList();

            var totalCount = Count(queryParameters);

            return new PagedResult<TEntity>(
                queryParameters.Page.Index,
                queryParameters.Page.Size,
                items,
                totalCount
            );
        }

        /// <summary>
        /// Gets paged list of models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>PagedResult{TModel}</cref>.</returns>
        public virtual PagedResult<TModel> GetPagedList<TModel>(QueryParameters<TEntity, TType> queryParameters)
        {
            var items = PagedResultQuery<TModel>(queryParameters).ToList();

            var totalCount = Count(queryParameters);

            return new PagedResult<TModel>(
                queryParameters.Page.Index,
                queryParameters.Page.Size,
                items,
                totalCount
            );
        }

        /// <summary>
        /// Asynchronously gets paged list of entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{PagedResult{TEntity}}</cref>.</returns>
        public virtual async Task<PagedResult<TEntity>> GetPagedListAsync(QueryParameters<TEntity, TType> queryParameters)
        {
            var items = await PagedResultQuery(queryParameters).ToListAsync();

            var totalCount = await CountAsync(queryParameters);

            return new PagedResult<TEntity>(
                queryParameters.Page.Index,
                queryParameters.Page.Size,
                items,
                totalCount
            );
        }

        /// <summary>
        /// Asynchronously gets paged list of models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{PagedResult{TModel}}</cref>.</returns>
        public virtual async Task<PagedResult<TModel>> GetPagedListAsync<TModel>(QueryParameters<TEntity, TType> queryParameters)
        {
            var items = await PagedResultQuery<TModel>(queryParameters).ToListAsync();

            var totalCount = await CountAsync(queryParameters);

            return new PagedResult<TModel>(
                queryParameters.Page.Index,
                queryParameters.Page.Size,
                items,
                totalCount
            );
        }

        /// <summary>
        /// Checks if any entity of type T satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public virtual bool Exists(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return Query(queryParameters).Any();
        }

        /// <summary>
        /// Asynchronously checks if any entity of type T satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public virtual async Task<bool> ExistsAsync(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await Query(queryParameters).AnyAsync();
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
        /// Returns a number that represents how many entities in repository satisfy a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns>A number that represents how many entities in repository satisfy a query parameters.</returns>
        public virtual int Count(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return CountQuery(queryParameters).Count();
        }

        /// <summary>
        /// Asynchronously returns a number that represents how many entities in repository satisfy a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns>A number that represents how many entities in repository satisfy a query parameters.</returns>
        public virtual async Task<int> CountAsync(QueryParameters<TEntity, TType> queryParameters = null)
        {
            return await CountQuery(queryParameters).CountAsync();
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
        /// Inserts entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Insert(TEntity entity)
        {
            return DbSet.Add(entity).Entity;
        }

        /// <summary>
        /// Inserts list of entities in the repository.
        /// </summary>
        /// <param name="entities">List of entities</param>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
        }

        /// <summary>
        /// Asynchronously inserts entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns><cref>TEntity</cref>.</returns>
        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            var entityEntry = await DbSet.AddAsync(entity);

            return entityEntry.Entity;
        }

        /// <summary>
        /// Asynchronously inserts list of entities in the repository.
        /// </summary>
        /// <param name="entities">List of entities</param>
        public virtual async Task InsertAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        /// <summary>
        /// Updates entity in the repository.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="startTrackProperties">if set to <c>true</c> marks entity as modified.</param>
        /// <returns>TEntity.</returns>
        public virtual TEntity Update(TEntity entity, bool startTrackProperties = false)
        {
            DbSet.Attach(entity);

            if (!startTrackProperties)
            {
                MarkAsModified(entity);
            }

            return entity;
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

        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Delete(TEntity entity)
        {
            if (IsDetached(entity))
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }

        /// <summary>
        /// Deletes list of entities by their ids.
        /// </summary>
        /// <param name="ids">List of entities ids.</param>
        public virtual void Delete(IEnumerable<TType> ids)
        {
            var entities = this.GetList(x => ids.Contains(x.Id));
            DbSet.RemoveRange(entities);
        }
    }
}