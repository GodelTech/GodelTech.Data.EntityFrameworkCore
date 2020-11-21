using System.Linq;

namespace GodelTech.Data.EntityFrameworkCore
{
    /// <summary>
    /// Interface of data mapper.
    /// </summary>
    public interface IDataMapper
    {
        /// <summary>
        /// Method to map from a queryable using the provided mapping engine.
        /// </summary>
        /// <typeparam name="TDestination">Destination type</typeparam>
        /// <param name="source">Queryable source</param>
        /// <returns>Expression to map into</returns>
        IQueryable<TDestination> Map<TDestination>(IQueryable source);
    }
}