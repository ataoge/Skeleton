using System;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus.Processor.States
{
    public class ScheduledState : IState
    {
        public const string StateName = "Scheduled";

        public TimeSpan? ExpiresAfter => null;

        public string Name => StateName;

        public void Apply(PublishedMessage message, IStorageTransaction transaction)
        {
        }

        public void Apply(ReceivedMessage message, IStorageTransaction transaction)
        {
        }
    }

    public class FailedState : IState
    {
        public const string StateName = "Failed";

        public TimeSpan? ExpiresAfter => TimeSpan.FromDays(15);

        public string Name => StateName;

        public void Apply(PublishedMessage message, IStorageTransaction transaction)
        {
        }

        public void Apply(ReceivedMessage message, IStorageTransaction transaction)
        {
        }
    }

    public class SucceededState : IState
    {
        public const string StateName = "Succeeded";

        public SucceededState()
        {
            ExpiresAfter = TimeSpan.FromHours(1);
        }

        public SucceededState(int expireAfterSeconds)
        {
            ExpiresAfter = TimeSpan.FromSeconds(expireAfterSeconds);
        }

        public TimeSpan? ExpiresAfter { get; }

        public string Name => StateName;

        public void Apply(PublishedMessage message, IStorageTransaction transaction)
        {
        }

        public void Apply(ReceivedMessage message, IStorageTransaction transaction)
        {
        }
    }
}