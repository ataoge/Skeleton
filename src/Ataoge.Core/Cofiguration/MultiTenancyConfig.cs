namespace Ataoge.Configuration
{
    /// <summary>
    /// Used to configure multi-tenancy.
    /// </summary>
    public class MultiTenancyConfig : IMultiTenancyConfig
    {
        /// <summary>
        /// Is multi-tenancy enabled?
        /// Default value: false.
        /// </summary>
        public bool IsEnabled { get; set; }
    }
}