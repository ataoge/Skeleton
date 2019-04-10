using System;

namespace Ataoge.EventBus.Diagnostics
{
    public class BrokerConsumeEventData : BrokerEventData
    {
        public BrokerConsumeEventData(Guid operationId, string operation, string brokerAddress,
            string brokerTopicName, string brokerTopicBody, DateTimeOffset startTime)
            : base(operationId, operation, brokerAddress, brokerTopicName, brokerTopicBody)
        {
            StartTime = startTime;
        }

        public DateTimeOffset StartTime { get; }
    }
}