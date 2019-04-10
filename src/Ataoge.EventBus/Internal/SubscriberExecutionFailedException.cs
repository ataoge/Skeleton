using System;

namespace Ataoge.EventBus.Internal
{
    internal class SubscriberExecutionFailedException : Exception
    {
        public SubscriberExecutionFailedException(string message, Exception ex) : base(message, ex)
        {
        }
    }
}