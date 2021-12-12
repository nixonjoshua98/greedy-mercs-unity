using System;

namespace GM
{
    public static class TimeSpan_Extensions
    {
        public static string Format(this TimeSpan ts, bool allowNegative = false)
        {
            if (allowNegative) return $"{ts.Hours}h {ts.Minutes}m {ts.Seconds}s";

            return $"{Math.Max(0, ts.Hours)}h {Math.Max(0, ts.Minutes)}m {Math.Max(0, ts.Seconds)}s";

        }
    }
}
