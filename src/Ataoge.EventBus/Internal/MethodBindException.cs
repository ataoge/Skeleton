using System;
using System.Runtime.Serialization;

namespace Ataoge.EventBus.Internal
{
    [Serializable]
    internal class MethodBindException : Exception
    {
        public MethodBindException()
        {
        }

        public MethodBindException(string message) : base(message)
        {
        }

        public MethodBindException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MethodBindException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}