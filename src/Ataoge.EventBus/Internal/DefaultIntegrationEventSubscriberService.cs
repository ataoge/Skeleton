using System;
using System.Threading.Tasks;
using Ataoge.EventBus.Abstractions;
using Ataoge.EventBus.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace Ataoge.EventBus.Internal
{
    public interface IIntergrationEventSubscriberService
    {
        void HandleEventMessageAsync(EventMessage message);
    }

    public class DefultIntergrationEventSubscriberService : IIntergrationEventSubscriberService, ISubscriber
    {
        public DefultIntergrationEventSubscriberService(IEventBusSubscriptionsManager manager, IServiceProvider serviceProvider, IContentSerializer serializer)
        {
            _subsManager = manager;
            _serviceProvider = serviceProvider;
            _serializer = serializer;
        }

        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly IContentSerializer _serializer;

        [Subscribe(DefaultEventBus.IntegrationEventRouteName)]
        public void HandleEventMessageAsync(EventMessage message)
        {
            ProcessEvent(message.EventTypeName, message.Content).GetAwaiter().GetResult();
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                 
                using (var scope = _serviceProvider.CreateScope())
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        if (subscription.IsDynamic)
                        { 
                            //var handler = scope.ServiceProvider.GetService(subscription.HandlerType) as IDynamicIntegrationEventHandler ; // scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                            //dynamic eventData = JObject.Parse(message);
                            //await handler.Handle(eventData);
                        }
                        else
                        {
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = _serializer.DeSerialize(message, eventType);
                            var handler = scope.ServiceProvider.GetService(subscription.HandlerType); //scope.ResolveOptional(subscription.HandlerType);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                        }
                    }
                }
            }
        }
    }
}