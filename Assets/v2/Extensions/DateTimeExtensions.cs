using System;

namespace GM
{
    public static class DateTimeExtensions
    {
        public static bool IsBetween(this DateTime dt, (DateTime, DateTime) dates)
        {
            return dt >= dates.Item1 && dt <= dates.Item2;
        }
    }
}
