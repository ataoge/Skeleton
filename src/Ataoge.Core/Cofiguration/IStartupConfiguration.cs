using Ataoge.Configuration;

namespace Ataoge.Configuration
{
    /// <summary>
    /// Used to configure ABP and modules on startup.
    /// </summary>
    public interface IStartupConfiguration 
    {
        /// <summary>
        /// Used to configure navigation.
        /// </summary>
        //INavigationConfiguration Navigation { get; }

         /// <summary>
        /// Used to configure multi-tenancy.
        /// </summary>
        IMultiTenancyConfig MultiTenancy { get; }

        /// <summary>
        /// Gets/sets default connection string used by ORM module.
        /// It can be name of a connection string in application's config file or can be full connection string.
        /// </summary>
        string DefaultNameOrConnectionString { get; set; }
        
        /// <summary>
        /// Used to configure modules.
        /// Modules can write extension methods to <see cref="IModuleConfigurations"/> to add module specific configurations.
        /// </summary>
        IModuleConfigurations Modules { get; }

        void Initialize();

        /// <summary>
        /// Gets a configuration object.
        /// </summary>
        T Get<T>();

    }
}