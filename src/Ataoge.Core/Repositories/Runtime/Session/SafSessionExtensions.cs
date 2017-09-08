using System;
using System.Reflection;
using Ataoge.Security;

namespace Ataoge.Runtime.Session
{
    /// <summary>
    /// Extension methods for <see cref="ISafSession"/>.
    /// </summary>
    public static class SafSessionExtensions
    {
        /// <summary>
        /// Gets current User's Id.
        /// Throws <see cref="SafException"/> if <see cref="ISafSession.UserId"/> is null.
        /// </summary>
        /// <param name="session">Session object.</param>
        /// <returns>Current User's Id.</returns>
        public static TKey GetUserId<TKey>(this ISafSession<TKey> session) where TKey : IEquatable<TKey>
        {
            if (session.UserId == null)
            {
                throw new SafException("Session.UserId is null! Probably, user is not logged in.");
            }

            return session.UserId;
        }

        /// <summary>
        /// Gets current Tenant's Id.
        /// Throws <see cref="SafException"/> if <see cref="ISafSession.TenantId"/> is null.
        /// </summary>
        /// <param name="session">Session object.</param>
        /// <returns>Current Tenant's Id.</returns>
        /// <exception cref="SafException"></exception>
        public static int GetTenantId<TKey>(this ISafSession<TKey> session) where TKey : IEquatable<TKey>
        {
            if (!session.TenantId.HasValue)
            {
                throw new SafException("Session.TenantId is null! Possible problems: No user logged in or current logged in user in a host user (TenantId is always null for host users).");
            }

            return session.TenantId.Value;
        }

        /// <summary>
        /// Creates <see cref="UserIdentifier"/> from given session.
        /// Returns null if <see cref="IAbpSession.UserId"/> is null.
        /// </summary>
        /// <param name="session">The session.</param>
        public static UserIdentifier<TKey> ToUserIdentifier<TKey>(this ISafSession<TKey> session) where TKey : IEquatable<TKey>
        {
            return session.UserId == null
                ? null
                : new UserIdentifier<TKey>(session.TenantId, session.GetUserId());
        }
    }
}