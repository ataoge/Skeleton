using Ataoge.MultiTenancy;
using Microsoft.EntityFrameworkCore;

namespace Ataoge.EntityFrameworkCore
{
    public interface IDbContextProvider<out TDbContext>
        where TDbContext : DbContext
    {
        TDbContext GetDbContext();

        TDbContext GetDbContext(MultiTenancySides? multiTenancySide );
    }
}