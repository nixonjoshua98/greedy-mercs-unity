using System;

namespace GM
{
    public static class TimeSpan_Extensions
    {
        // = Private = //
        static string Pad(int val) => $"{Math.Max(val, 0)}".PadLeft(2, '0');

        // = Extensions = //
        public static string Format(this TimeSpan ts) => $"{Pad(ts.Hours)}h {Pad(ts.Minutes)}m {Pad(ts.Seconds)}s";
    }
}
