using System;

namespace Ataoge.EventBus.Models
{
    public abstract class Message
    {
        public virtual string Id { get; set; }

        public virtual DateTime Timestamp { get; set; }

        public virtual string Content { get; set; }

        public virtual string CallbackName { get; set; }
    }
}