using Ataoge.EventBus.Abstractions;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus.Internal
{
    internal class DefaultMessagePacker : IMessagePacker
    {
        private readonly IContentSerializer _serializer;

        public DefaultMessagePacker(IContentSerializer serializer)
        {
            _serializer = serializer;
        }

        public string Pack(Message obj)
        {
            return _serializer.Serialize(obj);
        }

        public Message UnPack(string packingMessage)
        {
            return _serializer.DeSerialize<MessageDto>(packingMessage);
        }
    }
}