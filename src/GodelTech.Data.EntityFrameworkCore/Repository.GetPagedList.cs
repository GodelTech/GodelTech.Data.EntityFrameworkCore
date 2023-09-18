using System.Diagnostics.CodeAnalysis;
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
        public virtual PagedResult<TEntity> GetPagedList([NotNull] QueryParameters<TEntity, TKey> queryParameters)
        {
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
        public virtual PagedResult<TModel> GetPagedList<TModel>([NotNull] QueryParameters<TEntity, TKey> queryParameters)
        {
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
        public virtual async Task<PagedResult<TEntity>> GetPagedListAsync(
            [NotNull] QueryParameters<TEntity, TKey> queryParameters,
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

        /// <summary>
        /// Asynchronously gets paged list of models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns><cref>Task{PagedResult{TModel}}</cref>.</returns>
        public virtual async Task<PagedResult<TModel>> GetPagedListAsync<TModel>(
            [NotNull] QueryParameters<TEntity, TKey> queryParameters,
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
