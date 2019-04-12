using System;
using System.Collections.Generic;
using Ataoge.EventBus;
using Ataoge.EventBus.Abstractions;
using Ataoge.EventBus.Internal;
using Ataoge.EventBus.ModelBinding;
using Ataoge.EventBus.Processor;
using Ataoge.EventBus.Processor.States;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection" /> for configuring consistence services.
    /// </summary>
    public static class EventBusServiceCollectionExtensions
    {
        /// <summary>
        /// Adds and configures the consistence services for the consistency.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="CapOptions" />.</param>
        /// <returns>An <see cref="CapBuilder" /> for application services.</returns>
        public static ConfigBuilder AddEventBus(this IServiceCollection services, Action<ConfigOptions> setupAction)
        {
            if (setupAction == null)
            {
                throw new ArgumentNullException(nameof(setupAction));
            }

            services.TryAddSingleton<CapMarkerService>();

            //EventBus
            services.TryAddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            services.TryAddSingleton<IIntergrationEventSubscriberService, DefultIntergrationEventSubscriberService>();
            services.TryAddScoped<IEventBus, DefaultEventBus>();

            //Consumer service
            AddSubscribeServices(services);

            //Serializer and model binder
            services.TryAddSingleton<IContentSerializer, JsonContentSerializer>();
            services.TryAddSingleton<IMessagePacker, DefaultMessagePacker>();
            services.TryAddSingleton<IConsumerServiceSelector, DefaultConsumerServiceSelector>();
            services.TryAddSingleton<IModelBinderFactory, ModelBinderFactory>();

            services.TryAddSingleton<ICallbackMessageSender, CallbackMessageSender>();
            services.TryAddSingleton<IConsumerInvokerFactory, ConsumerInvokerFactory>();
            services.TryAddSingleton<MethodMatcherCache>();

            //Processors
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IProcessingServer, ConsumerHandler>());
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IProcessingServer, DefaultProcessingServer>());
            services.TryAddSingleton<IStateChanger, StateChanger>();

            //Queue's message processor
            services.TryAddSingleton<NeedRetryMessageProcessor>();

            //Sender and Executors   
            services.TryAddSingleton<IDispatcher, Dispatcher>();
            // Warning: IPublishMessageSender need to inject at extension project. 
            services.TryAddSingleton<ISubscriberExecutor, DefaultSubscriberExecutor>();

            

            //Options and extension service
            var options = new ConfigOptions();
            setupAction(options);
            foreach (var serviceExtension in options.Extensions)
            {
                serviceExtension.AddServices(services);
            }
            services.AddSingleton(options);

            //Startup and Middleware
            services.AddTransient<IHostedService, DefaultBootstrapper>();
            //services.AddTransient<IStartupFilter, StartupFilter>();

            return new ConfigBuilder(services);
        }

        private static void AddSubscribeServices(IServiceCollection services)
        {
            var consumerListenerServices = new List<KeyValuePair<Type, Type>>();
            foreach (var rejectedServices in services)
            {
                if (rejectedServices.ImplementationType != null
                    && typeof(ISubscriber).IsAssignableFrom(rejectedServices.ImplementationType))
                {
                    consumerListenerServices.Add(new KeyValuePair<Type, Type>(typeof(ISubscriber),
                        rejectedServices.ImplementationType));
                }
            }

            foreach (var service in consumerListenerServices)
            {
                services.TryAddEnumerable(ServiceDescriptor.Transient(service.Key, service.Value));
            }
        }
    }
}