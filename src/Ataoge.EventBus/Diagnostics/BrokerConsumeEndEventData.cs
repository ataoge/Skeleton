using System;

namespace Ataoge.EventBus.Diagnostics
{
    public class BrokerConsumeEndEventData : BrokerConsumeEventData
    {
        public BrokerConsumeEndEventData(Guid operationId, string operation, string brokerAddress,
            string brokerTopicName,
            string brokerTopicBody, DateTimeOffset startTime, TimeSpan duration)
            : base(operationId, operation, brokerAddress, brokerTopicName, brokerTopicBody, startTime)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; }
    }
}