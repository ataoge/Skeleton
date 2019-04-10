using System.Threading.Tasks;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus
{
    /// <summary>
    /// 消费执行器，执行接收到的消息
    /// </summary>
    public interface ISubscriberExecutor
    {
        Task<OperateResult> ExecuteAsync(ReceivedMessage message);
    }
}