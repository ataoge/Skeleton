using System.Threading.Tasks;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus
{
    /// <summary>
    /// 发布消息发送者。用来发送发布消息，具体实现通常保存要发送消息，然后在将发送消息发到消息队列。
    /// </summary>
    public interface IPublishMessageSender
    {
        Task<OperateResult> SendAsync(PublishedMessage message);
    }
}