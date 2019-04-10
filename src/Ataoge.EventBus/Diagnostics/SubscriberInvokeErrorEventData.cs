using System;

namespace Ataoge.EventBus.Diagnostics
{
    public class SubscriberInvokeErrorEventData : SubscriberInvokeEndEventData, IErrorEventData
    {
        public SubscriberInvokeErrorEventData(Guid operationId, string operation, string methodName,
            string subscribeName, string subscribeGroup, string parameterValues, Exception exception,
            DateTimeOffset startTime, TimeSpan duration) : base(operationId, operation, methodName, subscribeName,
            subscribeGroup, parameterValues, startTime, duration)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}