using UnityEngine;

namespace GM
{
    public static class MathUtils
    {
        static int RoundDownTo(int value, int multiple) => Mathf.FloorToInt(value / (float)multiple) * multiple;
        static int RoundUpTo(int value, int multiple) => RoundDownTo(value, multiple) + multiple;
        public static int NextMultipleMax(int value, int multiple, int maxValue) => Mathf.Min(Mathf.Min(maxValue, RoundUpTo(value, multiple)) - value);
        public static bool PercentChance(float percent) => percent >= Random.Range(0.0f, 1.0f);
    }
}