using System;

namespace Ataoge.EventBus.Models
{
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid().ToString("N");
            CreateTime = DateTime.UtcNow;
        }

        public string Id  { get; }
        public DateTime CreateTime { get; }
    }
}