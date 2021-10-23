using System;

namespace GM
{
    public static class TimeSpan_Extensions
    {
        public static string Format(this TimeSpan ts)
        {
            return $"{ts.Hours}h {ts.Minutes}m {ts.Seconds}s";
        }
    }
}
