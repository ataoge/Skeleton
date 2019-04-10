using System;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus.EfCore
{
    public class PublishedMessageEntity
    {
        public PublishedMessageEntity(PublishedMessage publishedMessage)
        {
            this.Id = publishedMessage.Id;
            this.Name = publishedMessage.Name;
            this.Content = publishedMessage.Content;
            this.Retries = publishedMessage.Retries;
            this.StatusName = publishedMessage.StatusName;
            this.Added = publishedMessage.Added;
            this.ExpiresAt = publishedMessage.ExpiresAt;
        }

        public long Id { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime Added { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public int Retries { get; set; }

        public string StatusName { get; set; }

        public string Version {get; set;}

        public override string ToString()
        {
            return "name:" + Name + ", content:" + Content;
        }
    }
}