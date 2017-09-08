using System;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Ataoge;
using Ataoge.Security;
using JetBrains.Annotations;

namespace System.Security.Claims
{
     public static class ClaimsIdentityExtensions
     {
        public static UserIdentifier<TKey> GetUserIdentifierOrNull<TKey>(this IIdentity identity) where TKey : IEquatable<TKey>
        {
            Check.NotNull(identity, nameof(identity));

            var userId = identity.GetUserId<TKey>();
            if (userId == null)
            {
                return null;
            }

            return new UserIdentifier<TKey>(identity.GetTenantId(), userId);
        }

        public static TKey GetUserId<TKey>([NotNull] this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var userIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdOrNull == null || string.IsNullOrWhiteSpace(userIdOrNull.Value))
            {
                return default(TKey);
            }

            return (TKey)Convert.ChangeType(userIdOrNull.Value, typeof(TKey));
        }

        public static int? GetTenantId(this IIdentity identity)
        {
            Check.NotNull(identity, nameof(identity));

            var claimsIdentity = identity as ClaimsIdentity;

            var tenantIdOrNull = claimsIdentity?.Claims?.FirstOrDefault(c => c.Type == SafClaimTypes.TenantId);
            if (tenantIdOrNull == null || string.IsNullOrWhiteSpace(tenantIdOrNull.Value))
            {
                return null;
            }

            return Convert.ToInt32(tenantIdOrNull.Value);
        }
     }
}