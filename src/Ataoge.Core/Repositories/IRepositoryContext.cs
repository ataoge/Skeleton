using System.Threading;
using System.Threading.Tasks;

namespace Ataoge.Repositories
{
    public interface IRepositoryContext
    {
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}