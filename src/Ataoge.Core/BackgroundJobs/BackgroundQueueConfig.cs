namespace Ataoge.BackgroundJobs
{
    public class BackgroundQueueConfig
    {
        public int MaxConcurrentCount{get; set;} = 1;

        public int MilllisecondsToWaitBeforePickingUpTask {get;set;} = 1000;
    }
}