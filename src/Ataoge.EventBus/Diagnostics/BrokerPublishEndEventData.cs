using System;

namespace Ataoge.EventBus.Diagnostics
{
    public class BrokerPublishEndEventData : BrokerPublishEventData
    {
        public BrokerPublishEndEventData(Guid operationId, string operation, string brokerAddress,
            string brokerTopicName,
            string brokerTopicBody, DateTimeOffset startTime, TimeSpan duration)
            : base(operationId, operation, brokerAddress, brokerTopicName, brokerTopicBody, startTime)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; }
    }
}