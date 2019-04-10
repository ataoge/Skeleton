using System;

namespace Ataoge.EventBus.Utilities
{
    internal static class DateTimeExtensions
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
        
        public static long ToTimestamp(DateTime value)
        {
            var elapsedTime = value - Epoch;
            return (long)elapsedTime.TotalSeconds;
        }


        public static DateTime FromTimestamp(long value)
        {
            return Epoch.AddSeconds(value);
        }
    }
}