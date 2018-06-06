using System;
using Ataoge;
using Ataoge.Modules;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class SkeletonServiceCollectionExtensions
    {
        public static IServiceCollection AddAtaogeSketleton<TStartupModule>(this IServiceCollection services, Action<SkeletonOptionsBuilder> optionsAction)
            where TStartupModule : ModuleBase
        {
            var optionsBuilder = new SkeletonOptionsBuilder();
            optionsBuilder.Services = services;

            optionsAction(optionsBuilder); 

            services.AddSingleton<SkeletonOptions>(optionsBuilder.Options);
            IModuleManager moduleManager = new ModuleManager(optionsBuilder.Options);
            moduleManager.Initialize(typeof(TStartupModule));
            moduleManager.ConfigModules(services);
            services.AddSingleton<IModuleManager>(moduleManager);

            return services;
        }
    }
}