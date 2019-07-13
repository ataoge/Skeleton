using Ataoge.EventBus.Dashboard;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DashboardServiceCollectionExtensions
    {
        public static void AddEventBusDashboard(this IServiceCollection services)
        {
            services.ConfigureOptions(typeof(DashboardConfigureOptions));
        }
    }
}