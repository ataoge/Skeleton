using System.Threading.Tasks;

namespace Ataoge.EventBus
{
    /// <summary>
    /// 发布消息的执行者。执行者发布消息到消息队列
    /// </summary>
    public interface IPublishExecutor
    {
        /// <summary>
        /// publish message to message queue.
        /// </summary>
        /// <param name="keyName">The message topic name.</param>
        /// <param name="content">The message content.</param>
        /// <returns></returns>
        Task<OperateResult> PublishAsync(string keyName, string content);
    }
}