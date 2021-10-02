using System;

public static class Utils
{
    public static DateTime UnixToDateTime(long ts) => DateTimeOffset.FromUnixTimeMilliseconds(ts).UtcDateTime;
}