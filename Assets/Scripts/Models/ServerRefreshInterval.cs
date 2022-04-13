﻿using System;

namespace GM.Models
{
    public class CurrentServerRefresh
    {
        public DateTime Previous;
        public DateTime Next;

        public CurrentServerRefresh(DateTime prev, DateTime next)
        {
            Previous = prev;
            Next = next;
        }

        public bool IsBetween(DateTime dt)
        {
            return Previous < dt && Next > dt;
        }
    }


    public class ServerRefreshInterval
    {
        public TimeSpan Interval;

        public int WeekDay = -1;
        public int MonthDate = -1;

        public int Hour;
        public int Minute;
        public int Second;

        public CurrentServerRefresh Current => RefreshPairFromDate(DateTime.UtcNow);

        private CurrentServerRefresh RefreshPairFromDate(DateTime dt)
        {
            DateTime refreshTime = new DateTime(dt.Year, dt.Month, dt.Day, Hour, Minute, Second, DateTimeKind.Utc);

            // Week Day (Mon, Tue etc.)
            if (WeekDay > -1)
            {
                while (refreshTime.DayOfWeek != (DayOfWeek)WeekDay)
                    refreshTime -= TimeSpan.FromDays(1);

                return new(refreshTime, refreshTime + TimeSpan.FromDays(7));
            }

            // Month date
            if (MonthDate > -1)
            {
                while (refreshTime.Day != MonthDate)
                    refreshTime -= TimeSpan.FromDays(1);

                DateTime nextRefresh = new(refreshTime.Year, refreshTime.Month + 1, refreshTime.Day, refreshTime.Hour, refreshTime.Minute, refreshTime.Second);

                return new(refreshTime, nextRefresh);
            }

            // General Interval (1 hour, 3 days etc)
            if (refreshTime > dt)
            {
                while (refreshTime > dt)
                    refreshTime -= Interval;

                return new(refreshTime, refreshTime + Interval);
            }
            else
            {
                while (dt > refreshTime)
                    refreshTime += Interval;

                return new(refreshTime - Interval, refreshTime);
            }
        }

        public DateTime Next => RefreshPairFromDate(DateTime.UtcNow).Next;
        public DateTime Previous => RefreshPairFromDate(DateTime.UtcNow).Previous;
        public TimeSpan TimeUntilNext => Next - DateTime.UtcNow;
    }
}
