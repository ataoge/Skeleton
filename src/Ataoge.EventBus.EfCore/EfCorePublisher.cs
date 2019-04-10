using System;
using System.Threading;
using System.Threading.Tasks;
using Ataoge.EventBus.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EventBus.EfCore
{
    internal class EfCorePublisher : PublisherBase, ICallbackPublisher
    {
        private readonly EfCoreOptions _options;
        public EfCorePublisher(IServiceProvider provider) : base(provider)
        {
             _options = provider.GetService<EfCoreOptions>();
        }

        public async Task PublishCallbackAsync(PublishedMessage message)
        {
            await PublishAsyncInternal(message);
        }

        protected override async Task ExecuteAsync(PublishedMessage message, ITransaction transaction,
            CancellationToken cancel = default(CancellationToken))
        {
            var dbContext = (DbContext)base.ServiceProvider.GetRequiredService(_options.DbContextType);
            
            if (NotUseTransaction)
            {
                dbContext.Entry(message).Property("Version").CurrentValue = _options.Version;
                dbContext.Set<PublishedMessage>().Add(message);
                await dbContext.SaveChangesAsync(cancel);
                return;
            }

            dbContext.Entry(message).Property("Version").CurrentValue = _options.Version;
            dbContext.Set<PublishedMessage>().Add(message);
            await dbContext.SaveChangesAsync(cancel);
        }

        private string PrepareSql()
        {
            return
                $"INSERT INTO \"published\" (\"Id\",\"Version\",\"Name\",\"Content\",\"Retries\",\"Added\",\"ExpiresAt\",\"StatusName\")VALUES(@Id,'{_options.Version}',@Name,@Content,@Retries,@Added,@ExpiresAt,@StatusName);";
        }

    }
}