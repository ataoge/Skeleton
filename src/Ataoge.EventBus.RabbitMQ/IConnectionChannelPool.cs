using RabbitMQ.Client;

namespace Ataoge.EventBus.RabbitMQ
{
    public interface IConnectionChannelPool
    {
        string HostAddress { get; }

        string Exchange { get; }

        IConnection GetConnection();

        IModel Rent();

        bool Return(IModel context);
    }
}