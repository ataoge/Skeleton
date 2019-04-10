using System;
using Ataoge.EventBus.Utilities;

namespace Ataoge.EventBus.Models
{
    public class MessageDto : Message
    {
        public MessageDto()
        {
            Id = ObjectId.GenerateNewStringId();
            Timestamp = DateTime.Now;
        }

        public MessageDto(string content) : this()
        {
            Content = content;
        }

        public override string Id { get; set; }

        public override DateTime Timestamp { get; set; }

        public override string Content { get; set; }

        public override string CallbackName { get; set; }
    }
}