using Ataoge.BackgroundJobs;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class BackgroundServiceExtensions
    {
        public static IServiceCollection AddBackgroundQueue(this IServiceCollection services)
        {
            services.AddSingleton<BackgroundQueue>();
            services.AddSingleton<IHostedService, BackgroundQueueService>();
            return services;
        }
    }
}