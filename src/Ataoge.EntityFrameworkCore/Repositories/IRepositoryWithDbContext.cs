using Microsoft.EntityFrameworkCore;

namespace Ataoge.EntityFrameworkCore.Repositories
{
    public interface IRepositoryWithDbContext
    {
        DbContext GetDbContext();
    }
}