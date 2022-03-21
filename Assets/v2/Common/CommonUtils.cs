using System;

namespace GM
{
    public static class CommonUtils
    {
        public static DateTime GetEstimateNextDailyRefresh(DateTime dt)
        {
            DateTime now = DateTime.UtcNow;

            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, 0);
            DateTime copy = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, 0);

            while (now >= copy)
            {
                copy = copy.AddDays(1);
            }

            return copy;
        }
    }
}
