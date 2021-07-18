using System.Linq;
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
        /// <returns><c>true</c> if exists, <c>false</c> otherwise.</returns>
        public virtual async Task<bool> ExistsAsync(QueryParameters<TEntity, TKey> queryParameters = null)
        {
            return await Query(queryParameters).AnyAsync();
        }
    }
}