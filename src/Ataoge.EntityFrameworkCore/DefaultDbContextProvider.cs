using System;
using Ataoge.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Ataoge.EntityFrameworkCore
{
    public class DefaultDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        public DefaultDbContextProvider(TDbContext dbContext, ILoggerFactory loggerFactory)
        {
            DbContext = dbContext;
            Repositories.EfCoreRepositoryHelper.Logger = loggerFactory.CreateLogger<DefaultDbContextProvider<TDbContext>>();
        }

        public TDbContext DbContext { get; private set; }

        public TDbContext GetDbContext()
        {
            return DbContext;
        }

        public TDbContext GetDbContext(MultiTenancySides? multiTenancySide)
        {
            return DbContext;
        }

      
    }
}