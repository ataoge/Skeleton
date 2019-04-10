using System.Threading.Tasks;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus
{
    /// <summary>
    /// A callback that is sent to Producer after a successful consumer execution
    /// </summary>
    public interface ICallbackPublisher
    {
         /// <summary>
        /// Publish a callback message
        /// </summary>
        Task PublishCallbackAsync(PublishedMessage obj);
    }
}