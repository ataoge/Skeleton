using System;
using Ataoge.EventBus.Abstractions;
using Ataoge.EventBus.Internal;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus
{
    internal class DefaultEventBus : IEventBus
    {

        public DefaultEventBus(IPublisher publisher, IEventBusSubscriptionsManager subsManager,  IContentSerializer serializer)
        {
            this._subsManager = subsManager;
            _publisher = publisher;
            _serializer = serializer;
        }

        private readonly IEventBusSubscriptionsManager _subsManager;
        //private readonly IServiceProvider _provider;

        private readonly IPublisher _publisher;
        private readonly IContentSerializer _serializer;

        internal const string IntegrationEventRouteName = "ataoge.messages.intergrationevent";
        
        public void Publish(IntegrationEvent @event)
        {
            _publisher.Publish(IntegrationEventRouteName, CreateEventMessage(@event));
        }

        private EventMessage CreateEventMessage(IntegrationEvent @event)
        {
            var msg = new EventMessage();
            msg.EventTypeName = @event.GetType().Name;
            msg.Content = _serializer.Serialize(@event);
            return msg;
        }

           public void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            DoInternalSubscription(eventName);
            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);
            _subsManager.AddSubscription<T, TH>();
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                
            }
        }


         public void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent
        {
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }
            
    }
}