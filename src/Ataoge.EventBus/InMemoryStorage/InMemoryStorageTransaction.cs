using System;
using System.Linq;
using System.Threading.Tasks;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus.InMemoryStorage
{
    internal class InMemoryStorageTransaction : IStorageTransaction
    {
        private InMemoryStorageConnection _connection;

        public InMemoryStorageTransaction(InMemoryStorageConnection inMemoryStorageConnection)
        {
            this._connection = inMemoryStorageConnection;
        }

        public void UpdateMessage(PublishedMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var msg = _connection.PublishedMessages.FirstOrDefault(x => message.Id == x.Id);
            if (msg == null) return;
            msg.Retries = message.Retries;
            msg.Content = message.Content;
            msg.ExpiresAt = message.ExpiresAt;
            msg.StatusName = message.StatusName;
        }

        public void UpdateMessage(ReceivedMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            var msg = _connection.ReceivedMessages.FirstOrDefault(x => message.Id == x.Id);
            if (msg == null) return;
            msg.Retries = message.Retries;
            msg.Content = message.Content;
            msg.ExpiresAt = message.ExpiresAt;
            msg.StatusName = message.StatusName;
        }

        public Task CommitAsync()
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

    }
}