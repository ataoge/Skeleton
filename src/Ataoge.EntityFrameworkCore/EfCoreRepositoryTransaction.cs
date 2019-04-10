using System;
using Ataoge.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ataoge.EntityFrameworkCore
{
    internal class EfCoreRepositoryTransaction : IRepositoryTransaction
    {
        public EfCoreRepositoryTransaction(IDbContextTransaction dbContextTransaction)
        {
            this._dbContextTransaction = dbContextTransaction;
        }
        private readonly IDbContextTransaction _dbContextTransaction;
        public Guid TransactionId => _dbContextTransaction.TransactionId;

        public void Commit()
        {
            _dbContextTransaction.Commit();
        }

        public void Dispose()
        {
            _dbContextTransaction.Dispose();
        }

        public void Rollback()
        {
            _dbContextTransaction.Rollback();
        }
    }
}