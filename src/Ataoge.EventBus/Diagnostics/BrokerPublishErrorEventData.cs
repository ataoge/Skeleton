

using System;

namespace Ataoge.EventBus.Diagnostics
{
    public class BrokerPublishErrorEventData : BrokerPublishEndEventData, IErrorEventData
    {
        public BrokerPublishErrorEventData(Guid operationId, string operation, string brokerAddress,
            string brokerTopicName, string brokerTopicBody, Exception exception, DateTimeOffset startTime,
            TimeSpan duration)
            : base(operationId, operation, brokerAddress, brokerTopicName, brokerTopicBody, startTime, duration)
        {
            Exception = exception;
        }

        public Exception Exception { get; }
    }
}