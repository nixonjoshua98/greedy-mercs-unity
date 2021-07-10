using System;
using System.IO;
using System.Text;
using System.Numerics;
using System.Collections;
using System.IO.Compression;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;



public static class FormatString
{
    static Dictionary<int, string> unitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" }, { 5, "Q" } };

    public static string Number(double val)
    {
        if (val < 1d)
            return Math.Round(val, 3).ToString();

        int n = (int)Math.Log(val, 1000);

        float m = (float)(val / Mathf.Pow(1000.0f, n));

        if (n < unitsTable.Count)
            return m.ToString("F") + unitsTable[n];

        return val.ToString("e2").Replace("+", "");
    }
}