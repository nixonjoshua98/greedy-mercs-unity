using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace GM.Common
{
    public static class Utility
    {
        public static System.Random SeededRandom(string seed)
        {
            using var algo = SHA1.Create();
            var hash = BitConverter.ToInt32(algo.ComputeHash(Encoding.UTF8.GetBytes(seed)));
            return new(hash);
        }

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
                return percent >= UnityEngine.Random.Range(0.0f, 1.0f);
            }
        }
    }
}
