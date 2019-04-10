using System.Threading;
using System.Threading.Tasks;
using Ataoge.EventBus.Monitoring;

namespace Ataoge.EventBus.InMemoryStorage
{
    internal class InMemoryStorage : IStorage
    {
        private readonly IStorageConnection _connection;

        public InMemoryStorage(IStorageConnection connection)
        {
            _connection = connection;
        }

        public IStorageConnection GetConnection()
        {
            return _connection;
        }

        public IMonitoringApi GetMonitoringApi()
        {
            return new InMemoryMonitoringApi(this);
        }

        public Task InitializeAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}