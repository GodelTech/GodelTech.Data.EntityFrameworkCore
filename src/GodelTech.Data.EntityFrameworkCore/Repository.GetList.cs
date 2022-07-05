using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore
{
    public partial class Repository<TEntity, TKey>
    {
        /// <summary>
        /// Gets entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IList{TEntity}</cref>.</returns>
        public virtual IList<TEntity> GetList(QueryParameters<TEntity, TKey> queryParameters = null)
        {
            return Query(queryParameters).ToList();
        }

        /// <summary>
        /// Gets models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>IList{TModel}</cref>.</returns>
        public virtual IList<TModel> GetList<TModel>(QueryParameters<TEntity, TKey> queryParameters = null)
        {
            return Query<TModel>(queryParameters).ToList();
        }

        /// <summary>
        /// Asynchronously gets entities of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{IList{TModel}}</cref>.</returns>
        public virtual async Task<IList<TEntity>> GetListAsync(QueryParameters<TEntity, TKey> queryParameters = null)
        {
            return await Query(queryParameters).ToListAsync();
        }

        /// <summary>
        /// Asynchronously gets models of type T from repository that satisfies a query parameters.
        /// </summary>
        /// <typeparam name="TModel">The type of the T model.</typeparam>
        /// <param name="queryParameters">Query parameters.</param>
        /// <returns><cref>Task{IList{TModel}}</cref>.</returns>
        public virtual async Task<IList<TModel>> GetListAsync<TModel>(QueryParameters<TEntity, TKey> queryParameters = null)
        {
            return await Query<TModel>(queryParameters).ToListAsync();
        }
    }
}
