using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.AspNetCore
{
    public static class SafServiceCollectionExtensions
    {
        /// <summary>
        /// Integrates ABP to AspNet Core.
        /// </summary>
        /// <typeparam name="TStartupModule">Startup module of the application which depends on other used modules. Should be derived from <see cref="AbpModule"/>.</typeparam>
        /// <param name="services">Services.</param>
        public static IServiceCollection AddAtaoge(this IServiceCollection services)
        {
            return services;
        }
    }
}