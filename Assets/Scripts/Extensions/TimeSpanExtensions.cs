using System;

namespace SRC
{
    public enum TimeSpanFormat
    {
        Default,
        Largest
    }

    public static class TimeSpanExtensions
    {
        public static string ToString(this TimeSpan ts, TimeSpanFormat format = TimeSpanFormat.Default)
        {
            switch (format)
            {
                case TimeSpanFormat.Largest:
                    if (ts.Days > 0) return $"{ts.Days} days";
                    if (ts.Hours > 0) return $"{ts.Hours} hours";
                    if (ts.Minutes > 0) return $"{ts.Minutes} minutes";
                    if (ts.Seconds > 0) return $"{ts.Seconds} seconds";
                    return "a few seconds";

                default:
                    return $"{ts.Hours}h {ts.Minutes}m {ts.Seconds}s";
            }
        }
    }
}
