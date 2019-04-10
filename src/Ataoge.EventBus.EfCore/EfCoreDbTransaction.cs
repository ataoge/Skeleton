using System;
using Ataoge.EventBus;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore.Storage
{
    internal class EfCoreDbTransaction : IDbContextTransaction
    {
        private readonly ITransaction _transaction;

        public EfCoreDbTransaction(ITransaction transaction)
        {
            _transaction = transaction;
            var dbContextTransaction = (IDbContextTransaction) _transaction.DbTransaction;
            TransactionId = dbContextTransaction.TransactionId;
        }
        public Guid TransactionId { get; }

        public void Commit()
        {
            _transaction.Commit();
        }

        public void Dispose()
        {
            _transaction.Dispose();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }
    }

    public static class EventBusTransactionExtensions
    {
        internal static ITransaction Begin(this ITransaction transaction,
            IDbContextTransaction dbTransaction, bool autoCommit = false)
        {
            transaction.DbTransaction = dbTransaction;
            transaction.AutoCommit = autoCommit;

            return transaction;
        }

        /// <summary>
        /// Start the CAP transaction
        /// </summary>
        /// <param name="database">The <see cref="DatabaseFacade" />.</param>
        /// <param name="publisher">The <see cref="IPublisher" />.</param>
        /// <param name="autoCommit">Whether the transaction is automatically committed when the message is published</param>
        /// <returns>The <see cref="IDbContextTransaction" /> of EF dbcontext transaction object.</returns>
        public static IDbContextTransaction BeginTransaction(this DatabaseFacade database,
            IPublisher publisher, bool autoCommit = false)
        {
            var trans = database.BeginTransaction();
            var capTrans = publisher.Transaction.Begin(trans, autoCommit);
            return new EfCoreDbTransaction(capTrans);
        }
    }
}