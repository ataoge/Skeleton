using System;
using System.Threading;
using System.Threading.Tasks;
using Ataoge.EventBus.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EventBus.InMemoryStorage
{
    internal class InMemoryPublisher : PublisherBase, ICallbackPublisher
    {
        public InMemoryPublisher(IServiceProvider provider) : base(provider)
        {
        }

        public async Task PublishCallbackAsync(PublishedMessage message)
        {
            await PublishAsyncInternal(message);
        }

        protected override Task ExecuteAsync(PublishedMessage message, ITransaction transaction,
            CancellationToken cancel = default(CancellationToken))
        {
            var connection = (InMemoryStorageConnection)ServiceProvider.GetService<IStorageConnection>();

            connection.PublishedMessages.Add(message);

            return Task.CompletedTask;
        }
    }
}