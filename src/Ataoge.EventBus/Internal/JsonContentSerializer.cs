using System;
using Ataoge.EventBus.Abstractions;
using Ataoge.EventBus.Utilities;

namespace Ataoge.EventBus.Internal
{
    internal class JsonContentSerializer : IContentSerializer
    {
        public T DeSerialize<T>(string messageObjStr)
        {
            return JsonHelper.FromJson<T>(messageObjStr);
        }

        public object DeSerialize(string content, Type type)
        {
            return JsonHelper.FromJson(content, type);
        }

        public string Serialize<T>(T messageObj)
        {
            return JsonHelper.ToJson(messageObj);
        }
    }
}