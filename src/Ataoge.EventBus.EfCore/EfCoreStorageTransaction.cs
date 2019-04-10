using System;
using System.Threading.Tasks;
using Ataoge.EventBus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EventBus.EfCore
{
    internal class EfCoreStorageTransaction : IStorageTransaction
    {
        IDbContextTransaction _dbContextTransaction;
        IServiceScope _scope;
        DbContext  _dbContext;
        
        public EfCoreStorageTransaction(EfCoreStorageConnection connection)
        {
            var options = connection.Options;
            _scope = connection.CreateScope();
            _dbContext = (DbContext)_scope.ServiceProvider.GetRequiredService(options.DbContextType);
            _dbContextTransaction = _dbContext.Database.BeginTransaction();
        }

        public Task CommitAsync()
        {
            _dbContextTransaction.Commit();
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
            _scope.Dispose();
        }

        public void UpdateMessage(PublishedMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            _dbContext.Set<PublishedMessage>().Update(message);
            _dbContext.SaveChanges();
        }

        public void UpdateMessage(ReceivedMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            _dbContext.Set<ReceivedMessage>().Update(message);
            _dbContext.SaveChanges();
        }
    }
}