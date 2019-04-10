using System;
using System.Collections.Generic;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus.Monitoring
{
    public interface IMonitoringApi
    {
        StatisticsDto GetStatistics();

        IList<MessageDto> Messages(MessageQueryDto queryDto);

        int PublishedFailedCount();

        int PublishedSucceededCount();

        int ReceivedFailedCount();

        int ReceivedSucceededCount();

        IDictionary<DateTime, int> HourlySucceededJobs(MessageType type);

        IDictionary<DateTime, int> HourlyFailedJobs(MessageType type);
    }
}