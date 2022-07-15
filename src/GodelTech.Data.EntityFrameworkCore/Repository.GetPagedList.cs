using System;
using System.Linq;
using System.Threading;
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
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns><cref>Task{PagedResult{TEntity}}</cref>.</returns>
        public virtual Task<PagedResult<TEntity>> GetPagedListAsync(
            QueryParameters<TEntity, TKey> queryParameters,
            CancellationToken cancellationToken = default)
        {
            if (queryParameters == null) throw new ArgumentNullException(nameof(queryParameters));

            return GetPagedListInternalAsync(queryParameters, cancellationToken);
        }

        /// <summary>
        /// Asynchronously gets paged list of models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns><cref>Task{PagedResult{TModel}}</cref>.</returns>
        public virtual Task<PagedResult<TModel>> GetPagedListAsync<TModel>(
            QueryParameters<TEntity, TKey> queryParameters,
            CancellationToken cancellationToken = default)
        {
            if (queryParameters == null) throw new ArgumentNullException(nameof(queryParameters));

            return GetPagedListInternalAsync<TModel>(queryParameters, cancellationToken);
        }

        private async Task<PagedResult<TEntity>> GetPagedListInternalAsync(
            QueryParameters<TEntity, TKey> queryParameters,
            CancellationToken cancellationToken = default)
        {
            var items = await PagedResultQuery(queryParameters).ToListAsync(cancellationToken);

            var totalCount = await CountAsync(queryParameters, cancellationToken);

            return new PagedResult<TEntity>(
                queryParameters.Page,
                items,
                totalCount
            );
        }

        private async Task<PagedResult<TModel>> GetPagedListInternalAsync<TModel>(
            QueryParameters<TEntity, TKey> queryParameters,
            CancellationToken cancellationToken = default)
        {
            var items = await PagedResultQuery<TModel>(queryParameters).ToListAsync(cancellationToken);

            var totalCount = await CountAsync(queryParameters, cancellationToken);

            return new PagedResult<TModel>(
                queryParameters.Page,
                items,
                totalCount
            );
        }
    }
}
