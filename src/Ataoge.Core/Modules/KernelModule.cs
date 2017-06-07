using System;
using Ataoge.Application.Navigation;
using Ataoge.Configuration;
using Ataoge.Infrastructure;
using Ataoge.Runtime.Session;
using Ataoge.Security;
using Ataoge.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ataoge.Modules
{
    public class KernelModule : ModuleBase
    {
        public KernelModule()
        {

        }


        protected override void OnConfiguringService(IServiceCollection services)
        {
             //services.AddSingleton<IStartupConfiguration>(sp => new StartupConfiguration(sp));

             services.AddSingleton<INavigationManager, NavigationManager>();
             
             CoreOptionsExtension coreOptionsExtension = GetOrCreateExtension<CoreOptionsExtension>();
             if (coreOptionsExtension.UserIndentifier != null)
             {
                var keyType = TypeUtils.FindGenericTypeArguments(coreOptionsExtension.UserIndentifier, typeof(IUserIdentifier<>));
                services.AddTransient(typeof(IUserNavigationManager<>).MakeGenericType(keyType), typeof(UserNavigationManager<>).MakeGenericType(keyType)); 
                services.AddSingleton(typeof(ISafSession<>).MakeGenericType(keyType), typeof(ClaimsSafSession<>).MakeGenericType(keyType));
             }
             services.TryAddSingleton<Infrastructure.INavigationConfiguration>(coreOptionsExtension.NavigationConfiguration);
        }


        protected override void OnConfiguredService(IServiceCollection services)
        {
            //services.TryAddSingleton<IModuleConfigurations, ModuleConfigurations>();
            //services.TryAddSingleton<IMultiTenancyConfig, MultiTenancyConfig>();
            //services.TryAddSingleton<INavigationConfiguration, NavigationConfiguration>();
            //services.AddSingleton<IModuleConfigurations, ModuleConfigurations>();
            //services.AddSingleton<IMultiTenancyConfig, MultiTenancyConfig>();
       
        }

        protected override void OnInitilize(IServiceProvider serviceProvider)
        {
            INavigationManager navigationManger = serviceProvider.GetRequiredService<INavigationManager>();
            navigationManger?.Initialize(serviceProvider);

        }
    }
}