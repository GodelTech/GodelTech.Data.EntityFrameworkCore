using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore
{
    public partial class Repository<TEntity, TKey>
    {
        /// <summary>
        /// Gets paged list of entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>PagedResult{TEntity}</cref>.</returns>
        public virtual PagedResult<TEntity> GetPagedList(QueryParameters<TEntity, TKey> queryParameters)
        {
            if (queryParameters == null) throw new ArgumentNullException(nameof(queryParameters));

            var items = PagedResultQuery(queryParameters).ToList();

            var totalCount = Count(queryParameters);

            return new PagedResult<TEntity>(
                queryParameters.Page,
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
        public virtual PagedResult<TModel> GetPagedList<TModel>(QueryParameters<TEntity, TKey> queryParameters)
        {
            if (queryParameters == null) throw new ArgumentNullException(nameof(queryParameters));

            var items = PagedResultQuery<TModel>(queryParameters).ToList();

            var totalCount = Count(queryParameters);

            return new PagedResult<TModel>(
                queryParameters.Page,
                items,
                totalCount
            );
        }

        /// <summary>
        /// Asynchronously gets paged list of entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{PagedResult{TEntity}}</cref>.</returns>
        public virtual Task<PagedResult<TEntity>> GetPagedListAsync(QueryParameters<TEntity, TKey> queryParameters)
        {
            if (queryParameters == null) throw new ArgumentNullException(nameof(queryParameters));

            return GetPagedListInternalAsync(queryParameters);
        }

        /// <summary>
        /// Asynchronously gets paged list of models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{PagedResult{TModel}}</cref>.</returns>
        public virtual Task<PagedResult<TModel>> GetPagedListAsync<TModel>(QueryParameters<TEntity, TKey> queryParameters)
        {
            if (queryParameters == null) throw new ArgumentNullException(nameof(queryParameters));

            return GetPagedListInternalAsync<TModel>(queryParameters);
        }

        private async Task<PagedResult<TEntity>> GetPagedListInternalAsync(QueryParameters<TEntity, TKey> queryParameters)
        {
            var items = await PagedResultQuery(queryParameters).ToListAsync();

            var totalCount = await CountAsync(queryParameters);

            return new PagedResult<TEntity>(
                queryParameters.Page,
                items,
                totalCount
            );
        }

        private async Task<PagedResult<TModel>> GetPagedListInternalAsync<TModel>(QueryParameters<TEntity, TKey> queryParameters)
        {
            var items = await PagedResultQuery<TModel>(queryParameters).ToListAsync();

            var totalCount = await CountAsync(queryParameters);

            return new PagedResult<TModel>(
                queryParameters.Page,
                items,
                totalCount
            );
        }
    }
}
