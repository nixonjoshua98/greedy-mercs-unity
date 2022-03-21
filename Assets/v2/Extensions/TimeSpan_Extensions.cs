using System;

namespace GM
{
    public enum TimeSpanFormat
    {
        Largest = 0
    }

    public static class TimeSpan_Extensions
    {
        // = Private = //
        static string Pad(int val) => $"{Math.Max(val, 0)}".PadLeft(2, '0');

        // = Extensions = //
        public static string Format(this TimeSpan ts, TimeSpanFormat format)
        {
            switch (format)
            {
                case TimeSpanFormat.Largest:
                    if (ts.Days > 0)    return $"{ts.Days} days";
                    if (ts.Hours > 0)   return $"{ts.Hours} hours";
                    if (ts.Minutes > 0) return $"{ts.Minutes} minutes";
                    if (ts.Seconds > 0) return $"{ts.Seconds} seconds";
                    return "a few seconds";
            }

            return "";
        }

        public static string Format(this TimeSpan ts) => $"{Pad(ts.Hours)}h {Pad(ts.Minutes)}m {Pad(ts.Seconds)}s";
    }
}
