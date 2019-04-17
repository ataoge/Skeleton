using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ataoge.EventBus.Monitoring;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ataoge.EventBus.EfCore
{
    internal class EfCoreStorage : IStorage
    {
        private readonly ConfigOptions _capOptions;
        private readonly ILogger _logger;
        private readonly EfCoreOptions _options;

        private readonly IServiceProvider _serviceProvider;

        public EfCoreStorage(ILogger<EfCoreStorage> logger,
            ConfigOptions capOptions,
            EfCoreOptions options,
            IServiceProvider serviceProvider)
        {
            _options = options;
            _logger = logger;
            _capOptions = capOptions;
            _serviceProvider = serviceProvider;
        }

        public IStorageConnection GetConnection()
        {
            return _serviceProvider.GetRequiredService<IStorageConnection>();
        }

        public IMonitoringApi GetMonitoringApi()
        {
            return new EfCoreMonitoringApi(_options, this);
        }

        public Task InitializeAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        internal T UseDbContext<T>(Func<DbContext, T> func)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService(_options.DbContextType);
                return func(dbContext);
            }
        }

        internal TResult UseDbContext<TEntity, TResult>(Func<IQueryable<TEntity>, TResult> func)
            where TEntity : class
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = (DbContext)scope.ServiceProvider.GetRequiredService(_options.DbContextType);
                var queryable = dbContext.Set<TEntity>();
                return func(queryable);
            }
        }
    }
}