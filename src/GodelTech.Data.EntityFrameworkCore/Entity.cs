using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GodelTech.Data.EntityFrameworkCore
{
    /// <summary>
    /// Entity for data layer.
    /// </summary>
    /// <typeparam name="TType">The type of the T type.</typeparam>
    /// <seealso>
    ///     <cref>GodelTech.Data.IEntity{TType}</cref>
    /// </seealso>
    public class Entity<TType> : IEntity<TType>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TType Id { get; set; }

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="x">First object to compare.</param>
        /// <param name="y">Second object to compare.</param>
        /// <returns>true if x object is equal to the y object; otherwise, false.</returns>
        public virtual bool Equals(IEntity<TType> x, IEntity<TType> y)
        {
            return x != null && y != null && x.Id.Equals(y.Id);
        }

        /// <summary>
        /// Returns a hash code for the instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public virtual int GetHashCode(IEntity<TType> obj)
        {
            return obj.Id.GetHashCode();
        }
    }
}
