using System;
using Ataoge.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Ataoge.EntityFrameworkCore
{
    public class DefaultDbContextProvider<TDbContext> : IDbContextProvider<TDbContext>
        where TDbContext : DbContext
    {
        public DefaultDbContextProvider(TDbContext dbContext)
        {
            DbContext = dbContext;
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