using System.Threading;
using System.Threading.Tasks;
using Ataoge.EventBus.Monitoring;
using Microsoft.Extensions.Logging;

namespace Ataoge.EventBus.EfCore
{
    internal class EfCoreStorage : IStorage
    {
        private readonly ConfigOptions _capOptions;
        private readonly ILogger _logger;
        private readonly EfCoreOptions _options;

        public EfCoreStorage(ILogger<EfCoreStorage> logger,
            ConfigOptions capOptions,
            EfCoreOptions options)
        {
            _options = options;
            _logger = logger;
            _capOptions = capOptions;
        }

        public IStorageConnection GetConnection()
        {
            throw new System.NotImplementedException();
        }

        public IMonitoringApi GetMonitoringApi()
        {
            throw new System.NotImplementedException();
        }

        public Task InitializeAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}