using Ataoge.EventBus.Models;

namespace Ataoge.EventBus.Abstractions
{
    /// <summary>
    /// 消息内容包装器
    /// <para>You can customize the message body filed name of the wrapper or add fields that you interested.</para>
    /// </summary>
    /// <remarks>
    /// We use the wrapper to provide some additional information for the message content,which is important for CAP。
    /// Typically, we may need to customize the field display name of the message,
    /// which includes interacting with other message components, which can be adapted in this manner
    /// </remarks>
    public interface IMessagePacker
    {
        /// <summary>
        /// Package a message object
        /// </summary>
        /// <param name="obj">The obj message to be packed.</param>
        string Pack(Message obj);

        /// <summary>
        /// Unpack a message strings to <see cref="Message" /> object.
        /// </summary>
        /// <param name="packingMessage">The string of packed message.</param>
        Message UnPack(string packingMessage);
    }
}