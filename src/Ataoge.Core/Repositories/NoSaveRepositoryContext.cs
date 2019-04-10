using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ataoge.Repositories
{
    public class NoSaveRepositoryContext : IRepositoryContext
    {
        public bool SupportSpatial => false;

        public IRepositoryTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public int SaveChanges()
        {
            return 0;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.FromResult(0);
        }
    }
}
