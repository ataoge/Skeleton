using System.Threading;
using System.Threading.Tasks;

namespace Ataoge.Repositories
{
    public interface IRepositoryContext
    {
        bool SupportSpatial {get;}
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}