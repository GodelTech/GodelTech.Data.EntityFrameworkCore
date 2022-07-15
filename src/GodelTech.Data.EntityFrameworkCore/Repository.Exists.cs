using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore
{
    public partial class Repository<TEntity, TKey>
    {
        /// <summary>
        /// Checks if any entity of type T satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public virtual bool Exists(QueryParameters<TEntity, TKey> queryParameters = null)
        {
            return Query(queryParameters).Any();
        }

        /// <summary>
        /// Asynchronously checks if any entity of type T satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public virtual async Task<bool> ExistsAsync(
            QueryParameters<TEntity, TKey> queryParameters = null,
            CancellationToken cancellationToken = default)
        {
            return await Query(queryParameters).AnyAsync(cancellationToken);
        }
    }
}
