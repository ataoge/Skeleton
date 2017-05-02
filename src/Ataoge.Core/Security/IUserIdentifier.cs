using System;

namespace Ataoge.Security
{
    /// <summary>
    /// Interface to get a user identifier.
    /// </summary>
    public interface IUserIdentifier<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Tenant Id. Can be null for host users.
        /// </summary>
        int? TenantId { get; }

        /// <summary>
        /// Id of the user.
        /// </summary>
        TKey UserId { get; }
    }

     public interface IUserIdentifier : IUserIdentifier<int>
     {

     }
}