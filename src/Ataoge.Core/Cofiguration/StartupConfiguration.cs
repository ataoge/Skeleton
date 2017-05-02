namespace Ataoge.Configuration
{
    /// <summary>
    /// This class is used to configure ABP and modules on startup.
    /// </summary>
    internal class StartupConfiguration :  IStartupConfiguration
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
        public StartupConfiguration()
        {
            //IocManager = iocManager;
        }

        public void Initialize()
        {
        }

    }
}