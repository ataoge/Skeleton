using System;
using System.Threading.Tasks;
using Ataoge.EventBus.Processor;
using Microsoft.Extensions.Logging;

namespace Ataoge.EventBus.EfCore
{
    internal class EfCoreCollectProcessor : ICollectProcessor
    {
        private const int MaxBatch = 1000;

        private static readonly string[] Tables =
        {
            "published", "received"
        };

        private readonly TimeSpan _delay = TimeSpan.FromSeconds(1);
        private readonly ILogger _logger;
        private readonly EfCoreOptions _options;
        private readonly TimeSpan _waitingInterval = TimeSpan.FromMinutes(5);

        public EfCoreCollectProcessor(ILogger<EfCoreCollectProcessor> logger,
            EfCoreOptions efCoreOptions)
        {
            _logger = logger;
            _options = efCoreOptions;
        }

        public async Task ProcessAsync(ProcessingContext context)
        {
            foreach (var table in Tables)
            {
                _logger.LogDebug($"Collecting expired data from table [{table}].");

                var removedCount = 0;
                do
                {
                    //using (var connection = new NpgsqlConnection(_options.ConnectionString))
                    //{
                    //    removedCount = await connection.ExecuteAsync(
                    //        $"DELETE FROM \"{_options.Schema}\".\"{table}\" WHERE \"ExpiresAt\" < @now AND \"Id\" IN (SELECT \"Id\" FROM \"{_options.Schema}\".\"{table}\" LIMIT @count);",
                    //        new {now = DateTime.Now, count = MaxBatch});
                    //}

                    if (removedCount != 0)
                    {
                        await context.WaitAsync(_delay);
                        context.ThrowIfStopping();
                    }
                } while (removedCount != 0);
            }

            await context.WaitAsync(_waitingInterval);
        }
    }
}