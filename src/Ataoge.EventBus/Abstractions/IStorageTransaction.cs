using System;
using System.Threading.Tasks;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus
{
    /// <summary>
    /// A transactional database storage operation.
    /// Update message state of the message table with transactional.
    /// </summary>
    public interface IStorageTransaction : IDisposable
    {
        void UpdateMessage(PublishedMessage message);

        void UpdateMessage(ReceivedMessage message);

        Task CommitAsync();
    }
}