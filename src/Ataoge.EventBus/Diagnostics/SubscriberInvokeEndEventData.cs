using System;

namespace Ataoge.EventBus.Diagnostics
{
    public class SubscriberInvokeEndEventData : SubscriberInvokeEventData
    {
        public SubscriberInvokeEndEventData(Guid operationId, string operation,
            string methodName, string subscribeName, string subscribeGroup,
            string parameterValues, DateTimeOffset startTime, TimeSpan duration)
            : base(operationId, operation, methodName, subscribeName, subscribeGroup, parameterValues, startTime)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; }
    }
}