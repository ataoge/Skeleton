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
        public INavigationConfiguration Navigation { get; private set; }


        /// <summary>
        /// Used to configure multi-tenancy.
        /// </summary>
        public IMultiTenancyConfig MultiTenancy { get; private set; }

         /// <summary>
        /// Private constructor for singleton pattern.
        /// </summary>
        public StartupConfiguration(IServiceProvider serviceProvider)
        {
            //IocManager = iocManager;
            this._serviceProvider = serviceProvider;
            MultiTenancy = (IMultiTenancyConfig)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(MultiTenancyConfig));
            Navigation = (INavigationConfiguration)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(NavigationConfiguration));
        }

        private readonly IServiceProvider _serviceProvider;

        public void Initialize()
        {
            
        }

        public T Get<T>()
        {
            return GetOrCreate(typeof(T).FullName, () => (T)ActivatorUtilities.CreateInstance(_serviceProvider, typeof(T)));
        }

    }
}