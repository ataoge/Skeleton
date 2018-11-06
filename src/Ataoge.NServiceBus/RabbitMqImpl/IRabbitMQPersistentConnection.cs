using System;
using RabbitMQ.Client;

namespace Ataoge.EventBus.RabbitMQImpl
{
    public interface IRabbitMQPersistentConnection
        : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}