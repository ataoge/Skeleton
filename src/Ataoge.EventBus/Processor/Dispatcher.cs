using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Ataoge.EventBus.Models;
using Microsoft.Extensions.Logging;

namespace Ataoge.EventBus.Processor
{
    internal class Dispatcher : IDispatcher, IDisposable
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        private readonly ISubscriberExecutor _executor;
        private readonly ILogger<Dispatcher> _logger;

        private readonly BlockingCollection<PublishedMessage> _publishedMessageQueue =
            new BlockingCollection<PublishedMessage>(new ConcurrentQueue<PublishedMessage>());

        private readonly BlockingCollection<ReceivedMessage> _receivedMessageQueue =
            new BlockingCollection<ReceivedMessage>(new ConcurrentQueue<ReceivedMessage>());

        private readonly IPublishMessageSender _sender;

        public Dispatcher(ILogger<Dispatcher> logger,
            IPublishMessageSender sender,
            ISubscriberExecutor executor)
        {
            _logger = logger;
            _sender = sender;
            _executor = executor;

            Task.Factory.StartNew(Sending, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            Task.Factory.StartNew(Processing, _cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public void EnqueueToPublish(PublishedMessage message)
        {
            _publishedMessageQueue.Add(message);
        }

        public void EnqueueToExecute(ReceivedMessage message)
        {
            _receivedMessageQueue.Add(message);
        }

        public void Dispose()
        {
            _cts.Cancel();
        }

        private void Sending()
        {
            try
            {
                while (!_publishedMessageQueue.IsCompleted)
                {
                    if (_publishedMessageQueue.TryTake(out var message, 100, _cts.Token))
                    {
                        try
                        {
                            _sender.SendAsync(message);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"An exception occurred when sending a message to the MQ. Topic:{message.Name}, Id:{message.Id}");
                        }
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // expected
            }
        }

        private void Processing()
        {
            try
            {
                foreach (var message in _receivedMessageQueue.GetConsumingEnumerable(_cts.Token))
                {
                    _executor.ExecuteAsync(message);
                }
            }
            catch (OperationCanceledException)
            {
                // expected
            }
        }
    }
}