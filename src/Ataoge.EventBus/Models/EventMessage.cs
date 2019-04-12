using System;

namespace Ataoge.EventBus.Models
{
    public class EventMessage
    {
        public EventMessage()
        {
            EventId = Guid.NewGuid().ToString("N");
            CreateTime = DateTime.UtcNow;
        }
        public string EventId {get; set;}

        public string EventTypeName {get; set;}

        public DateTime CreateTime {get; set;}

        public string Content {get; set;}
    }
}