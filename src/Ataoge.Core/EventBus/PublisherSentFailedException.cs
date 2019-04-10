using System;

namespace Ataoge.EventBus
{
    public class PublisherSentFailedException : Exception
    {
        public PublisherSentFailedException(string message) : base(message)
        {
        }

        public PublisherSentFailedException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}