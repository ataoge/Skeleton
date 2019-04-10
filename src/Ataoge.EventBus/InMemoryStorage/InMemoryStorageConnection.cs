using System;
using System.Collections.Async;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ataoge.EventBus.Models;
using Ataoge.EventBus.Utilities;

namespace Ataoge.EventBus.InMemoryStorage
{
    internal class InMemoryStorageConnection : IStorageConnection
    {
        private readonly ConfigOptions _capOptions;

        public InMemoryStorageConnection(ConfigOptions capOptions)
        {
            _capOptions = capOptions;

            PublishedMessages = new List<PublishedMessage>();
            ReceivedMessages = new List<ReceivedMessage>();
        }

        internal List<PublishedMessage> PublishedMessages { get; }

        internal List<ReceivedMessage> ReceivedMessages { get; }

        public IStorageTransaction CreateTransaction()
        {
           return new InMemoryStorageTransaction(this);
        }

        public Task<PublishedMessage> GetPublishedMessageAsync(long id)
        {
            return PublishedMessages.ToAsyncEnumerable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PublishedMessage>> GetPublishedMessagesOfNeedRetry()
        {
            return await PublishedMessages.ToAsyncEnumerable()
                .Where(x => x.Retries < _capOptions.FailedRetryCount
                         && x.Added < DateTime.Now.AddSeconds(-10)
                         && (x.StatusName == StatusName.Scheduled || x.StatusName == StatusName.Failed))
                .Take(200)
                .ToListAsync();
        }

        public void StoreReceivedMessage(ReceivedMessage message)
        {
            ReceivedMessages.Add(message);
        }

        public Task<ReceivedMessage> GetReceivedMessageAsync(long id)
        {
            return ReceivedMessages.ToAsyncEnumerable().FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ReceivedMessage>> GetReceivedMessagesOfNeedRetry()
        {
            return await ReceivedMessages.ToAsyncEnumerable()
                .Where(x => x.Retries < _capOptions.FailedRetryCount
                            && x.Added < DateTime.Now.AddSeconds(-10)
                            && (x.StatusName == StatusName.Scheduled || x.StatusName == StatusName.Failed))
                .Take(200)
                .ToListAsync();
        }

        public bool ChangePublishedState(long messageId, string state)
        {
            var msg = PublishedMessages.First(x => x.Id == messageId);
            msg.Retries++;
            msg.ExpiresAt = null;
            msg.StatusName = state;
            return true;
        }

        public bool ChangeReceivedState(long messageId, string state)
        {
            var msg = ReceivedMessages.First(x => x.Id == messageId);
            msg.Retries++;
            msg.ExpiresAt = null;
            msg.StatusName = state;
            return true;
        }
    }
}