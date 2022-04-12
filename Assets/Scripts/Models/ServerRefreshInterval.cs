using System;

namespace GM.Models
{
    public class ServerRefreshInterval
    {
        public TimeSpan Interval;

        public int WeekDay = -1;
        public int MonthDate = -1;

        public int Hour;
        public int Minute;
        public int Second;

        private (DateTime, DateTime) RefreshPairFromDate(DateTime dt)
        {
            DateTime refreshTime = new DateTime(dt.Year, dt.Month, dt.Day, Hour, Minute, Second);

            // Week Day (Mon, Tue etc.)
            if (WeekDay > -1)
            {
                while (refreshTime.DayOfWeek != (DayOfWeek)WeekDay)
                    refreshTime -= TimeSpan.FromDays(1);

                return (refreshTime, refreshTime + TimeSpan.FromDays(7));
            }

            // Month date
            if (MonthDate > -1)
            {
                while (refreshTime.Day != MonthDate)
                    refreshTime -= TimeSpan.FromDays(1);

                DateTime nextRefresh = new DateTime(refreshTime.Year, refreshTime.Month + 1, refreshTime.Day, refreshTime.Hour, refreshTime.Minute, refreshTime.Second);

                return (refreshTime, nextRefresh);
            }

            // General Interval (1 hour, 3 days etc)
            if (refreshTime > dt)
            {
                while (refreshTime > dt)
                    refreshTime -= Interval;

                return (refreshTime, refreshTime + Interval);
            }
            else
            {
                while (dt > refreshTime)
                    refreshTime += Interval;

                return (refreshTime - Interval, refreshTime);
            }
        }

        public DateTime Next => RefreshPairFromDate(DateTime.UtcNow).Item2;
        public DateTime Previous => RefreshPairFromDate(DateTime.UtcNow).Item1;
        public (DateTime Previous, DateTime Next) PreviousNextReset => RefreshPairFromDate(DateTime.UtcNow);
        public TimeSpan TimeUntilNext => Next - DateTime.UtcNow;
        public TimeSpan TimeSincePrevious => DateTime.UtcNow - Previous;
    }
}
