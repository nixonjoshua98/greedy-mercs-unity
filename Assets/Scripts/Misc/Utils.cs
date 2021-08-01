using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public static class Funcs
{
    // = = = Time = = = //
    public static DateTime FromUnixMs(long ts)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(ts).UtcDateTime;
    }

}