using System;
using Ataoge.EventBus;
using Ataoge.EventBus.RabbitMQ;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RabbitMQConfigOptionsExtensions
    {
        public static ConfigOptions UseRabbitMQ(this ConfigOptions options, string hostName)
        {
            return options.UseRabbitMQ(opt => { opt.HostName = hostName; });
        }

        public static ConfigOptions UseRabbitMQ(this ConfigOptions options, Action<RabbitMQOptions> configure)
        {
            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            options.RegisterExtension(new RabbitMQConfigOptionsExtension(configure));

            return options;
        }
    }
}
