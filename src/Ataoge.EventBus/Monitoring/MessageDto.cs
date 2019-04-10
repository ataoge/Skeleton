using System;

namespace Ataoge.EventBus.Monitoring
{
    public class MessageDto
    {
        public long Id { get; set; }

        public string Version { get; set; }

        public string Group { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime Added { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public int Retries { get; set; }

        public string StatusName { get; set; }
    }
}