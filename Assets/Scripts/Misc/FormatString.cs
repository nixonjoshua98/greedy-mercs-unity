using System;
using System.IO;
using System.Text;
using System.Numerics;
using System.Collections;
using System.IO.Compression;
using System.Collections.Generic;

using UnityEngine;



public static class FormatString
{
    readonly static Dictionary<int, string> units = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" }, { 5, "Q" } };

    public static string Number(double val, string prefix = "")
    {
        if (val < 1d) // Value is less than 1.0 so we just return it rounded
            return string.Format("{0}{1}", Math.Round(val, 3), prefix);

        int n = (int)Math.Log(val, 1000);

        float m = (float)(val / Mathf.Pow(1000.0f, n));

        if (n < units.Count) // Value is within the stored units
            return string.Format("{0}{1}{2}", m.ToString("F"), units[n], prefix);

        // Value is larger than the units provded, so return a exponent/mantissa
        return string.Format("{0}{1}", val.ToString("e2").Replace("+", ""), prefix);
    }
}