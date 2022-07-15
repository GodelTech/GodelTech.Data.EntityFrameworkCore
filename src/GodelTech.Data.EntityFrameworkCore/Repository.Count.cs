using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore
{
    public partial class Repository<TEntity, TKey>
    {
        /// <summary>
        /// Returns a number that represents how many entities in repository satisfy a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns>A number that represents how many entities in repository satisfy a query parameters.</returns>
        public virtual int Count(QueryParameters<TEntity, TKey> queryParameters = null)
        {
            return CountQuery(queryParameters).Count();
        }

        /// <summary>
        /// Asynchronously returns a number that represents how many entities in repository satisfy a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>A number that represents how many entities in repository satisfy a query parameters.</returns>
        public virtual async Task<int> CountAsync(
            QueryParameters<TEntity, TKey> queryParameters = null,
            CancellationToken cancellationToken = default)
        {
            return await CountQuery(queryParameters).CountAsync(cancellationToken);
        }
    }
}
