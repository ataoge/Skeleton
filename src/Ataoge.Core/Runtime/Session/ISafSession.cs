using System;
using Ataoge.MultiTenancy;

namespace Ataoge.Runtime.Session
{
    /// <summary>
    /// Defines some session information that can be useful for applications.
    /// </summary>
    public interface ISafSession<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets current UserId or null.
        /// It can be null if no user logged in.
        /// </summary>
        TKey UserId { get; }

        /// <summary>
        /// Gets current TenantId or null.
        /// This TenantId should be the TenantId of the <see cref="UserId"/>.
        /// It can be null if given <see cref="UserId"/> is a host user or no user logged in.
        /// </summary>
        int? TenantId { get; }

        /// <summary>
        /// Gets current multi-tenancy side.
        /// </summary>
        MultiTenancySides MultiTenancySide { get; }
    }
}
