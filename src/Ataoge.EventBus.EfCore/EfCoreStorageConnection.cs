using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ataoge.EventBus.Models;
using Ataoge.EventBus.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EventBus.EfCore
{
    internal class EfCoreStorageConnection : IStorageConnection
    {
        private readonly ConfigOptions _configOptions;
        private readonly IServiceProvider _serviceProvider;

        public EfCoreStorageConnection(EfCoreOptions options, ConfigOptions configOptions, IServiceProvider serviceProvider)
        {
            _configOptions = configOptions;
            _serviceProvider = serviceProvider;
            Options = options;
        }

        public EfCoreOptions Options { get; }

        public IStorageTransaction CreateTransaction()
        {
            return new EfCoreStorageTransaction(this);
        }

        internal IServiceScope CreateScope()
        {
            return _serviceProvider.CreateScope();
        }

        public bool ChangePublishedState(long messageId, string state)
        {
            throw new System.NotImplementedException();
        }

        public bool ChangeReceivedState(long messageId, string state)
        {
            throw new System.NotImplementedException();
        }

        public async Task<PublishedMessage> GetPublishedMessageAsync(long id)
        {
            using (var scope = CreateScope())
            {
                var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService(Options.DbContextType);
                return await dbContext.Set<PublishedMessage>().FirstOrDefaultAsync(t => t.Id == id);
            }
        }

        public async Task<IEnumerable<PublishedMessage>> GetPublishedMessagesOfNeedRetry()
        {
            var fourMinsAgo = DateTime.Now.AddMinutes(-4);
            using (var scope = CreateScope())
            {
                var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService(Options.DbContextType);
                var messages = dbContext.Set<PublishedMessage>().Where(t => t.Retries < _configOptions.FailedRetryCount && t.Added < fourMinsAgo && (t.StatusName == StatusName.Failed || t.StatusName == StatusName.Scheduled)).Take(200);

                return await messages.ToListAsync();
            }
        }

        public async Task<ReceivedMessage> GetReceivedMessageAsync(long id)
        {
            using (var scope = CreateScope())
            {
                var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService(Options.DbContextType);
                return await dbContext.Set<ReceivedMessage>().FirstOrDefaultAsync(t => t.Id == id);
            }
        }

        public async Task<IEnumerable<ReceivedMessage>> GetReceivedMessagesOfNeedRetry()
        {
            var fourMinsAgo = DateTime.Now.AddMinutes(-4);
            using (var scope = CreateScope())
            {
                var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService(Options.DbContextType);
                var messages = dbContext.Set<ReceivedMessage>().Where(t => t.Retries < _configOptions.FailedRetryCount && t.Added < fourMinsAgo && (t.StatusName == StatusName.Failed || t.StatusName == StatusName.Scheduled)).Take(200);
                return await messages.ToListAsync();
            }
        }

        public void StoreReceivedMessage(ReceivedMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            using (var scope = CreateScope())
            {
                var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService(Options.DbContextType);
                dbContext.Entry(message).Property("Version").CurrentValue = Options.Version;
                dbContext.Set<ReceivedMessage>().Add(message);
                dbContext.SaveChanges();
            }
        }
    }
}