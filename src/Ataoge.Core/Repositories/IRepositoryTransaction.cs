using System;

namespace Ataoge.Repositories
{
    public interface IRepositoryTransaction : IDisposable
    {
        Guid TransactionId { get; }

        void Commit();
        void Rollback();
        
    }

    public class NoopTransaction : IRepositoryTransaction
    {
        public Guid TransactionId => default(Guid);

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}