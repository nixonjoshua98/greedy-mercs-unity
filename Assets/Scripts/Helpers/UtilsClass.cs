using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class UtilsClass : MonoBehaviour
{
    public static string DoubleToString(double val)
    {
        if (val < 1d)
            return "0.0";

        Dictionary<int, string> unitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" } };

        int n = (int)Math.Log(val, 1000);

        float m = (float)(val / Mathf.Pow(1000.0f, n));

        string unit;

        if (n < unitsTable.Count)
            unit = unitsTable[n];

        else
        {
            int unitInt = n - unitsTable.Count;

            int secondUnit = unitInt % 26;

            int firstUnit = unitInt / 26;

            unit = Convert.ToChar(firstUnit + 97).ToString() + Convert.ToChar(secondUnit + 97).ToString();
        }

        return (Mathf.Floor(m * 100.0f) / 100.0f).ToString("0.#") + unit;
    }
}
