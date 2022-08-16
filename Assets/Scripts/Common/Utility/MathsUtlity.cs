using UnityEngine;

namespace SRC.Common
{
    public static class MathsUtlity
    {
        private static int RoundDownTo(int value, int multiple)
        {
            return Mathf.FloorToInt(value / (float)multiple) * multiple;
        }

        private static int RoundUpTo(int value, int multiple)
        {
            return RoundDownTo(value, multiple) + multiple;
        }

        public static int NextMultipleMax(int value, int multiple, int maxValue)
        {
            return Mathf.Min(Mathf.Min(maxValue, RoundUpTo(value, multiple)) - value);
        }

        public static bool PercentChance(float percent)
        {
            return percent >= Random.Range(0.0f, 1.0f);
        }
    }
}
