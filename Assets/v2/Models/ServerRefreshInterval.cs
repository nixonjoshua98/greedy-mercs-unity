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

        public DateTime NextRefreshAfter(DateTime dt)
        {
            DateTime refreshTime = new DateTime(dt.Year, dt.Month, dt.Day, Hour, Minute, Second);

            // Week Day (Mon, Tue etc.)
            if (WeekDay > -1)
            {
                while (refreshTime.DayOfWeek != (DayOfWeek)WeekDay)
                {
                    refreshTime -= TimeSpan.FromDays(1);
                }

                return refreshTime + TimeSpan.FromDays(7);

            }
            
            // Month date
            if (MonthDate > -1)
            {
                while (refreshTime.Day != MonthDate)
                {
                    refreshTime -= TimeSpan.FromDays(1);
                }

                return new DateTime(refreshTime.Year, refreshTime.Month + 1, refreshTime.Day, refreshTime.Hour, refreshTime.Minute, refreshTime.Second);
            }

            // General Interval (1 hour, 3 days etc)
            while (refreshTime < dt)
            {
                refreshTime += Interval;
            }

            return refreshTime;
        }

        public DateTime Next => NextRefreshAfter(DateTime.UtcNow);
        public TimeSpan TimeUntilNext => Next - DateTime.UtcNow;
    }
}
