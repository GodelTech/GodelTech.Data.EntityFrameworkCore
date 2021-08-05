using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GodelTech.Data.EntityFrameworkCore
{
    /// <summary>
    /// Entity with non database generated identifier for data layer.
    /// </summary>
    /// <typeparam name="TKey">The type of the T key.</typeparam>
    /// <seealso>
    ///     <cref>GodelTech.Data.IEntity{TKey}</cref>
    /// </seealso>
    public abstract class EntityNoneDatabaseGeneratedIdentifier<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public virtual TKey Id { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="x">First object to compare.</param>
        /// <param name="y">Second object to compare.</param>
        /// <returns>true if x object is equal to the y object; otherwise, false.</returns>
        public virtual bool Equals(IEntity<TKey> x, IEntity<TKey> y)
        {
            // Check whether the compared objects reference the same data
            if (ReferenceEquals(x, y)) return true;

            // Check whether any of the compared objects is null
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;

            // Check whether the objects' properties are equal.
            return x.Id.Equals(y.Id);
        }

        /// <summary>
        /// Returns a hash code for the instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public virtual int GetHashCode(IEntity<TKey> obj)
        {
            // Check whether the object is null
            if (ReferenceEquals(obj, null)) return 0;

            // Calculate the hash code for the object.
            return obj.Id.GetHashCode();
        }
    }
}