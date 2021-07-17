using Microsoft.EntityFrameworkCore;

namespace GodelTech.Data.EntityFrameworkCore
{
    /// <summary>
    /// Class DbContextBase.
    /// </summary>
    /// <seealso cref="DbContext" />
    public abstract class DbContextBase : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbContextBase"/> class.
        /// </summary>
        /// <param name="options">The database context options.</param>
        /// <param name="schemaName">Name of the schema.</param>
        protected DbContextBase(DbContextOptions options, string schemaName)
            : base(options)
        {
            SchemaName = schemaName;
        }

        /// <summary>
        /// Database schema name
        /// </summary>
        public string SchemaName
        {
            get;
        }
    }
}