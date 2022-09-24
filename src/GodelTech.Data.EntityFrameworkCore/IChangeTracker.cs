using System;

namespace GodelTech.Data.EntityFrameworkCore
{
    /// <summary>
    /// Interface to work with DbContext ChangeTracker.
    /// </summary>
    /// <seealso cref="IDisposable" />
    public interface IChangeTracker : IDisposable
    {
        /// <summary>
        /// Stops tracking all currently tracked entities.
        /// </summary>
        void ClearChangeTracker();
    }
}
