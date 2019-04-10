using System;

namespace Ataoge.EventBus.Internal
{
    internal class SubscriberNotFoundException : Exception
    {
        public SubscriberNotFoundException()
        {
        }

        public SubscriberNotFoundException(string message) : base(message)
        {
        }

        public SubscriberNotFoundException(string message, Exception inner) :
            base(message, inner)
        {
        }
    }
}