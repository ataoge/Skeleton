using System;

namespace Ataoge.EventBus.Diagnostics
{
    public class BrokerPublishEventData : BrokerEventData
    {
        public BrokerPublishEventData(Guid operationId, string operation, string brokerAddress,
            string brokerTopicName, string brokerTopicBody, DateTimeOffset startTime)
            : base(operationId, operation, brokerAddress, brokerTopicName, brokerTopicBody)
        {
            StartTime = startTime;
        }

        public DateTimeOffset StartTime { get; }
    }
}