using System;
using Ataoge.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EventBus.RabbitMQ
{
    internal class RabbitMQConfigOptionsExtension : IConfigOptionsExtension
    {
        private Action<RabbitMQOptions> _configure;

        public RabbitMQConfigOptionsExtension(Action<RabbitMQOptions> configure)
        {
            this._configure = configure;
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddSingleton<CapMessageQueueMakerService>();

            var options = new RabbitMQOptions();
            _configure?.Invoke(options);
            services.AddSingleton(options);

            services.AddSingleton<IConsumerClientFactory, RabbitMQConsumerClientFactory>();
            services.AddSingleton<IConnectionChannelPool, ConnectionChannelPool>();
            services.AddSingleton<IPublishExecutor, RabbitMQPublishMessageSender>();
            services.AddSingleton<IPublishMessageSender, RabbitMQPublishMessageSender>();
        }
    }
}