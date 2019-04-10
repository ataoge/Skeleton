using System;

namespace Ataoge.EventBus
{
    /// <summary>
    /// CAP Transaction wrapper, used to wrap database transactions, provides a consistent user interface
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// A flag is used to indicate whether the transaction is automatically committed after the message is published
        /// </summary>
        bool AutoCommit { get; set; }

        /// <summary>
        /// Database transaction object, can be converted to a specific database transaction object or IDBTransaction when used
        /// </summary>
        object DbTransaction { get; set; }

        /// <summary>
        /// Submit the transaction context of the CAP, we will send the message to the message queue at the time of submission
        /// </summary>
        void Commit();

        /// <summary>
        /// 当前事务失败将，删除存储在缓存中的数据
        /// We will delete the message data that has not been store in the buffer data of current transaction context.
        /// </summary>
        void Rollback();
    }
}