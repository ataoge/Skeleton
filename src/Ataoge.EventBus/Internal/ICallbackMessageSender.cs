using System.Threading.Tasks;

namespace Ataoge.EventBus.Internal
{
    internal interface ICallbackMessageSender
    {
        Task SendAsync(string messageId, string topicName, object bodyObj);
    }
}