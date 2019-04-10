using Ataoge.EventBus.Abstractions;

namespace Ataoge.EventBus
{
    /// <summary>
    /// An attribute for subscribe event bus message.
    /// </summary>
    public class SubscribeAttribute : TopicAttribute
    {
        public SubscribeAttribute(string name)
            : base(name)
        {

        }

        public override string ToString()
        {
            return Name;
        }
    }
}