using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.Configuration
{
    /// <summary>
    /// This class is used to configure ABP and modules on startup.
    /// </summary>
    internal class StartupConfiguration : DictionaryBasedConfig, IStartupConfiguration
    {
        /// <summary>
        /// Used to configure navigation.
        /// </summary>
        //public INavigationConfiguration Navigation { get; private set; }


        /// <summary>
        /// Used to configure multi-tenancy.
        /// </summary>
        public IMultiTenancyConfig MultiTenancy { get; private set; }

        /// <summary>
        /// Gets/sets default connection string used by ORM module.
        /// It can be name of a connection string in application's config file or can be full connection string.
        /// </summary>
        public string DefaultNameOrConnectionString { get; set; }

        /// <summary>
        /// Used to configure modules.
        /// Modules can write extension methods to <see cref="ModuleConfigurations"/> to add module specific configurations.
        /// </summary>
        public IModuleConfigurations Modules { get; private set; }

         /// <summary>
        /// Private constructor for singleton pattern.
        /// </summary>
        public StartupConfiguration(IServiceProvider serviceProvider)
        {
            //IocManager = iocManager;
            this._serviceProvider = serviceProvider;
            //MultiTenancy = (IMultiTenancyConfig)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(MultiTenancyConfig));
            //Navigation = (INavigationConfiguration)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(NavigationConfiguration));
        }

        private readonly IServiceProvider _serviceProvider;

        public void Initialize()//(IServiceProvider serviceProvider)
        {
            
            MultiTenancy = (IMultiTenancyConfig)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, typeof(MultiTenancyConfig));
            //Navigation = (INavigationConfiguration)_serviceProvider.GetService<INavigationConfiguration>();//ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, typeof(NavigationConfiguration));
            Modules = (IModuleConfigurations)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, typeof(ModuleConfigurations));
        }

        public T Get<T>()
        {
            return GetOrCreate(typeof(T).FullName, () => (T)ActivatorUtilities.GetServiceOrCreateInstance(_serviceProvider, typeof(T)));
        }

    }
}