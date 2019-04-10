using System;
using Ataoge.EventBus.Processor;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EventBus.EfCore
{
    internal class EfCoreConfigOptionsExtension : IConfigOptionsExtension
    {
        private readonly Action<EfCoreOptions> _configure;

        public EfCoreConfigOptionsExtension(Action<EfCoreOptions> configure)
        {
            _configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<CapStorageMarkerService>();
            services.AddSingleton<IStorage, EfCoreStorage>();
            services.AddSingleton<IStorageConnection, EfCoreStorageConnection>();

            services.AddScoped<IPublisher, EfCorePublisher>();
            services.AddScoped<ICallbackPublisher, EfCorePublisher>();

            services.AddTransient<ICollectProcessor, EfCoreCollectProcessor>();
            services.AddTransient<TransactionBase, EfCoreTransaction>();

            AddSingletonEfCoreOptions(services);
        }

        private void AddSingletonEfCoreOptions(IServiceCollection services)
        {
            var efCoreOptions = new EfCoreOptions();
            _configure(efCoreOptions);

            if (efCoreOptions.DbContextType != null)
            {
                services.AddSingleton(x => {
                    //var dbContext = (Microsoft.EntityFrameworkCore.DbContext)x.GetRequiredService(efCoreOptions.DbContextType);
                    
                    return  efCoreOptions;
                });
            }
            else
            {
                services.AddSingleton(efCoreOptions);
            }
        }
    }
}