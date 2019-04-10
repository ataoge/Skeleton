using System;
using Ataoge.EventBus;
using Ataoge.EventBus.EfCore;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EfCoreConfitOptionsExtensions
    {
        public static ConfigOptions UseEntityFramework<TContext>(this ConfigOptions options)
            where TContext : DbContext
        {
            return options.UseEntityFramework<TContext>(opt => { });
        }

        public static ConfigOptions UseEntityFramework<TContext>(this ConfigOptions options, Action<EfCoreOptions> configure)
            where TContext : DbContext
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new EfCoreConfigOptionsExtension(x =>
            {
                configure(x);
                x.Version = options.Version;
                x.DbContextType = typeof(TContext);
            }));

            return options;
        }
    }
}