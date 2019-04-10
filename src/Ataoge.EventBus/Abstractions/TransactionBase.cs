using System.Collections.Generic;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus
{
    public abstract class TransactionBase : ITransaction
    {
        protected TransactionBase(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
            _bufferList = new List<PublishedMessage>(1);
        }

        private readonly IDispatcher _dispatcher;

        private readonly IList<PublishedMessage> _bufferList;

        public bool AutoCommit { get; set; }
        public object DbTransaction { get; set; }

        protected internal virtual void AddToSent(PublishedMessage msg)
        {
            _bufferList.Add(msg);
        }

        /// <summary>
        /// 清空事务中的缓存，将发送队列添加给调度（收发器）
        /// </summary>
        protected virtual void Flush()
        {
            foreach (var message in _bufferList)
            {
                _dispatcher.EnqueueToPublish(message);
            }

            _bufferList.Clear();
        }

        public abstract void Commit();

        public abstract void Rollback();

        public abstract void Dispose();
    }
}