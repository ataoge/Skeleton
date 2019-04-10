using System;
using System.Threading.Tasks;
using Ataoge.EventBus.Processor;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EventBus.InMemoryStorage
{
    internal class InMemoryCollectProcessor : ICollectProcessor
    {
        private readonly ILogger _logger;
        private readonly TimeSpan _waitingInterval = TimeSpan.FromMinutes(5);

        public InMemoryCollectProcessor(ILogger<InMemoryCollectProcessor> logger)
        {
            _logger = logger;
        }

        public async Task ProcessAsync(ProcessingContext context)
        {
            _logger.LogDebug($"Collecting expired data from memory list.");

            var connection = (InMemoryStorageConnection)context.Provider.GetService<IStorageConnection>();

            connection.PublishedMessages.RemoveAll(x => x.ExpiresAt < DateTime.Now);
            connection.ReceivedMessages.RemoveAll(x => x.ExpiresAt < DateTime.Now);

            await context.WaitAsync(_waitingInterval);
        }
    }
}