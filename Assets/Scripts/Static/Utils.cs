using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public static class Funcs
{
    public static Vector3 AveragePosition(List<Vector3> ls)
    {
        Vector3 avg = Vector3.zero;

        foreach (Vector3 pos in ls)
            avg += pos;

        return avg / ls.Count;
    }


    public static string BonusString(BonusType bonusType)
    {
        return bonusType.ToString();
    }

    // = = = Time = = = //
    public static DateTime ToDateTime(long ts)
    {
        return DateTimeOffset.FromUnixTimeMilliseconds(ts).UtcDateTime;
    }

}