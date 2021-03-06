using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Ataoge.EventBus.Processor
{
    public class InfiniteRetryProcessor : IProcessor
    {
        private readonly IProcessor _inner;
        private readonly ILogger _logger;

        public InfiniteRetryProcessor(
            IProcessor inner,
            ILoggerFactory loggerFactory)
        {
            _inner = inner;
            _logger = loggerFactory.CreateLogger<InfiniteRetryProcessor>();
        }

        public async Task ProcessAsync(ProcessingContext context)
        {
            while (!context.IsStopping)
            {
                try
                {
                    await _inner.ProcessAsync(context);
                }
                catch (OperationCanceledException)
                {
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(1, ex, "Processor '{ProcessorName}' failed. Retrying...", _inner.ToString());
                }
            }
        }

        public override string ToString()
        {
            return _inner.ToString();
        }
    }
}