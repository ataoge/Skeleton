using Ataoge.EventBus.Models;

namespace Ataoge.EventBus
{
    /// <summary>
    /// 调度、收发接口
    /// </summary>
    public interface IDispatcher
    {
        void EnqueueToPublish(PublishedMessage message);

        void EnqueueToExecute(ReceivedMessage message);
    }
}