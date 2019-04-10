using Ataoge.EventBus.Processor;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EventBus.InMemoryStorage
{
    internal class InMemoryConfigOptionsExtension : IConfigOptionsExtension
    {
        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<CapStorageMarkerService>();
            services.AddSingleton<IStorage, InMemoryStorage>();
            services.AddSingleton<IStorageConnection, InMemoryStorageConnection>();

            services.AddSingleton<IPublisher, InMemoryPublisher>();
            services.AddSingleton<ICallbackPublisher, InMemoryPublisher>();

            services.AddTransient<ICollectProcessor, InMemoryCollectProcessor>();
            services.AddTransient<TransactionBase, InMemoryTransaction>();
        }
    }
}