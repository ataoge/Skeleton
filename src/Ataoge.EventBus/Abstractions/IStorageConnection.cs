using System.Collections.Generic;
using System.Threading.Tasks;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus
{
    
    /// <summary>
    /// Represents a connection to the storage.
    /// </summary>
    public interface IStorageConnection
    {
        //Sent messages

        /// <summary>
        /// Returns the message with the given id.
        /// </summary>
        /// <param name="id">The message's id.</param>
        Task<PublishedMessage> GetPublishedMessageAsync(long id);

        /// <summary>
        /// Returns executed failed messages.
        /// </summary>
        Task<IEnumerable<PublishedMessage>> GetPublishedMessagesOfNeedRetry();

        // Received messages

        /// <summary>
        /// Stores the message.
        /// </summary>
        /// <param name="message">The message to store.</param>
        void StoreReceivedMessage(ReceivedMessage message);

        /// <summary>
        /// Returns the message with the given id.
        /// </summary>
        /// <param name="id">The message's id.</param>
        Task<ReceivedMessage> GetReceivedMessageAsync(long id);

        /// <summary>
        /// Returns executed failed message.
        /// </summary>
        Task<IEnumerable<ReceivedMessage>> GetReceivedMessagesOfNeedRetry();

        /// <summary>
        /// Creates and returns an <see cref="IStorageTransaction" />.
        /// </summary>
        IStorageTransaction CreateTransaction();

        /// <summary>
        /// Change specified message's state of published message
        /// </summary>
        /// <param name="messageId">Message id</param>
        /// <param name="state">State name</param>
        bool ChangePublishedState(long messageId, string state);

        /// <summary>
        /// Change specified message's state  of received message
        /// </summary>
        /// <param name="messageId">Message id</param>
        /// <param name="state">State name</param>
        bool ChangeReceivedState(long messageId, string state);
   
    }
}