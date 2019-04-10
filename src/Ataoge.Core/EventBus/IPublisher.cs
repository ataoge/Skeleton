using System.Threading;
using System.Threading.Tasks;

namespace Ataoge.EventBus
{
    /// <summary>
    /// A publish service for publish a message to CAP.
    /// </summary>
    public interface IPublisher
    {
        
        /// <summary>
        /// CAP transaction context object
        /// </summary>
        ITransaction Transaction { get; }

        /// <summary>
        /// Asynchronous publish an object message.
        /// </summary>
        /// <param name="name">the topic name or exchange router key.</param>
        /// <param name="contentObj">message body content, that will be serialized of json.</param>
        /// <param name="callbackName">callback subscriber name</param>
        /// <param name="cancellationToken"></param>
        Task PublishAsync<T>(string name, T contentObj, string callbackName = null, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Publish an object message.
        /// </summary>
        /// <param name="name">the topic name or exchange router key.</param>
        /// <param name="contentObj">message body content, that will be serialized of json.</param>
        /// <param name="callbackName">callback subscriber name</param>
        void Publish<T>(string name, T contentObj, string callbackName = null);
    }
}