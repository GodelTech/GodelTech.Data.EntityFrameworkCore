using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore
{
    public partial class Repository<TEntity, TKey>
    {
        /// <summary>
        /// Gets entity of type T from repository that satisfies a query parameters.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>TEntity</cref>.</returns>
#pragma warning disable CA1716 // Identifiers should not match keywords
        public virtual TEntity Get(QueryParameters<TEntity, TKey> queryParameters = null)
#pragma warning restore CA1716 // Identifiers should not match keywords
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
#pragma warning disable CA1716 // Identifiers should not match keywords
        public virtual TModel Get<TModel>(QueryParameters<TEntity, TKey> queryParameters = null)
#pragma warning restore CA1716 // Identifiers should not match keywords
        {
            return Query<TModel>(queryParameters).FirstOrDefault();
        }

        /// <summary>
        /// Asynchronously gets entity of type T from repository that satisfies a query parameters.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns><cref>Task{TEntity}</cref>.</returns>
        public virtual async Task<TEntity> GetAsync(
            QueryParameters<TEntity, TKey> queryParameters = null,
            CancellationToken cancellationToken = default)
        {
            return await Query(queryParameters).FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Asynchronously gets model of type T from repository that satisfies a query parameters.
        /// If no entity is found, then null is returned.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns><cref>Task{TModel}</cref>.</returns>
        public virtual async Task<TModel> GetAsync<TModel>(
            QueryParameters<TEntity, TKey> queryParameters = null,
            CancellationToken cancellationToken = default)
        {
            return await Query<TModel>(queryParameters).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
