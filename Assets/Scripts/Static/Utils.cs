﻿using System;
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

namespace GM.Utils
{
    public class Lerp
    {
        public static IEnumerator RectTransform(RectTransform rt, Vector3 start, Vector3 end, float dur)
        {
            float progress = 0.0f;

            rt.localScale = start;

            while (progress < 1.0f)
            {
                progress += (Time.deltaTime / dur);

                rt.localScale = Vector3.Lerp(start, end, progress);

                yield return new WaitForEndOfFrame();
            }

            rt.localScale = Vector3.one;
        }
    }
}