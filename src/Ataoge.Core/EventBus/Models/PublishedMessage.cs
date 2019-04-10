using System;

namespace Ataoge.EventBus.Models
{
    public class PublishedMessage
    {
        /// <summary>
        /// Initializes a new instance of <see cref="PublishedMessage" />.
        /// </summary>
        public PublishedMessage()
        {
            Added = DateTime.Now;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime Added { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public int Retries { get; set; }

        public string StatusName { get; set; }

        public override string ToString()
        {
            return "name:" + Name + ", content:" + Content;
        }
    }
}