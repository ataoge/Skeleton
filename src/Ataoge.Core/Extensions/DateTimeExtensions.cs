namespace System
{
    public static class AtaogeDateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public static Tuple<DateTime, DateTime> ThisWeekRange(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            DateTime startWeek = dateTime.StartOfWeek(startOfWeek);
            DateTime endWeek = startWeek.AddDays(7);
            return Tuple.Create(startWeek, endWeek);
        }

        public static Tuple<DateTime, DateTime> LastWeekRange(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            DateTime endWeek = dateTime.StartOfWeek(startOfWeek);
            DateTime startWeek = endWeek.AddDays(-7);
            return Tuple.Create(startWeek, endWeek);
        }

        public static Tuple<DateTime, DateTime> LastTwoWeekRange(this DateTime dateTime, DayOfWeek startOfWeek = DayOfWeek.Sunday)
        {
            DateTime startWeek = dateTime.StartOfWeek(startOfWeek).AddDays(-14);
            DateTime endWeek = startWeek.AddDays(7); 
            return Tuple.Create(startWeek, endWeek);
        }

        public static DateTime StartOfMonth(this DateTime dt)
        {
            int diff  = dt.Day - 1;
            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime StartOfYear(this DateTime dt)
        {
            int diff  = dt.DayOfYear - 1;
            return dt.AddDays(-1 * diff).Date;
        }
    }
}