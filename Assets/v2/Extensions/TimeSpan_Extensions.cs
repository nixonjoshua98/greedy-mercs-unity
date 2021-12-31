using System;

namespace GM
{
    public static class TimeSpan_Extensions
    {
        public static string Format(this TimeSpan ts) => $"{Math.Max(0, ts.Hours)}h {Math.Max(0, ts.Minutes)}m {Math.Max(0, ts.Seconds)}s";
    }
}
