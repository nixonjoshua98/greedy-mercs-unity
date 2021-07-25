using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GM
{
    public static class MathUtils
    {
        public static int RoundDownTo(int value, int multiple) => Mathf.FloorToInt(value / (float)multiple) * multiple;
        public static int RoundUpTo(int value, int multiple) => RoundDownTo(value, multiple) + multiple;
        public static int NextMultipleMax(int value, int multiple, int maxValue) => Mathf.Min(Mathf.Min(maxValue, RoundUpTo(value, multiple)) - value);
    }
}
