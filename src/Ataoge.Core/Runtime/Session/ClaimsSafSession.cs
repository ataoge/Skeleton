using System;
using System.Linq;
using System.Security.Claims;
using Ataoge.Configuration;
using Ataoge.Security;
using Microsoft.Extensions.Options;

namespace Ataoge.Runtime.Session
{
    /// <summary>
    /// Implements <see cref="IAbpSession"/> to get session properties from current claims.
    /// </summary>
    public class ClaimsSafSession<TKey> : SafSessionBase<TKey>
        where TKey : IEquatable<TKey>
    {
        public override TKey UserId
        {
            get
            {
                var userIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdClaim?.Value))
                {
                    return default(TKey);
                }

                return (TKey)Convert.ChangeType(userIdClaim, typeof(TKey));
            }
        }

        public override int? TenantId
        {
            get
            {
               
                var tenantIdClaim = PrincipalAccessor.Principal?.Claims.FirstOrDefault(c => c.Type == SafClaimTypes.TenantId);
                if (!string.IsNullOrEmpty(tenantIdClaim?.Value))
                {
                    return Convert.ToInt32(tenantIdClaim.Value);
                }

                if (UserId == null)
                {
                    //Resolve tenant id from request only if user has not logged in!
                    //return TenantResolver.ResolveTenantId();
                }
                
                return null;
            }
        }

        

        protected IPrincipalAccessor PrincipalAccessor { get; }
        //protected ITenantResolver TenantResolver { get; }

        public ClaimsSafSession(
            IPrincipalAccessor principalAccessor,
            IOptions<MultiTenancyConfig> multiTenancy
           )
            : base(multiTenancy)
        {
            //TenantResolver = tenantResolver;
            PrincipalAccessor = principalAccessor;
        }
    }
}