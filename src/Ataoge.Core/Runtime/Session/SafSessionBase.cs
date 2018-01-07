using System;
using Ataoge.Configuration;
using Ataoge.MultiTenancy;
using Microsoft.Extensions.Options;

namespace Ataoge.Runtime.Session
{
    public abstract class SafSessionBase<TKey> : ISafSession<TKey>
        where TKey : IEquatable<TKey>
    {
        public IMultiTenancyConfig MultiTenancy { get; }
        
        public abstract TKey UserId { get; }

        public abstract int? TenantId { get; }

        public virtual MultiTenancySides MultiTenancySide
        {
            get
            {
                return MultiTenancy.IsEnabled && !TenantId.HasValue
                    ? MultiTenancySides.Host
                    : MultiTenancySides.Tenant; 
            }
        }

         protected SafSessionBase(IOptions<MultiTenancyConfig> multiTenancyOptions)
         {
            this.MultiTenancy = multiTenancyOptions.Value;
         }
    }
}