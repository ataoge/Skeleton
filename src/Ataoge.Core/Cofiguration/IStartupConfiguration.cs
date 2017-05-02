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
        INavigationConfiguration Navigation { get; }

         /// <summary>
        /// Used to configure multi-tenancy.
        /// </summary>
        IMultiTenancyConfig MultiTenancy { get; }

    }
}