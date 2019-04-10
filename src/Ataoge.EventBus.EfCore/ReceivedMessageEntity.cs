using System;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus.EfCore
{
    public class ReceivedMessageEntity
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ReceivedMessage" />.
        /// </summary>
        public ReceivedMessageEntity()
        {
            
        }

        public ReceivedMessageEntity(ReceivedMessage message)
        {
            this.Group = message.Group;
            this.Id = message.Id;
            this.Name = message.Name;
            this.Content = message.Content;
            this.Retries = message.Retries;
            this.StatusName = message.StatusName;
            this.Added = message.Added;
            this.ExpiresAt = message.ExpiresAt;
        }

        public long Id { get; set; }

        public string Group { get; set; }

        public string Name { get; set; }

        public string Content { get; set; }

        public DateTime Added { get; set; }

        public DateTime? ExpiresAt { get; set; }

        public int Retries { get; set; }

        public string StatusName { get; set; }

        public ReceivedMessage ToReceivedMessage()
        {
            return new ReceivedMessage
            {
                Group = Group,
                Id = Id,
                Name = Name,
                Content = Content,
                Retries = Retries,
                Added = Added,
                ExpiresAt = ExpiresAt
            };
        }

        public override string ToString()
        {
            return "name:" + Name + ", group:" + Group + ", content:" + Content;
        }
    }
}