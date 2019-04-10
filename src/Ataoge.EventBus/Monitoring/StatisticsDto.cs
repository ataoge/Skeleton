namespace Ataoge.EventBus.Monitoring
{
    public class StatisticsDto
    {
        public int Servers { get; set; }

        public int PublishedSucceeded { get; set; }

        public int ReceivedSucceeded { get; set; }

        public int PublishedFailed { get; set; }
        
        public int ReceivedFailed { get; set; }
    }
}