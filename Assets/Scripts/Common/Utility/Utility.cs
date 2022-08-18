using System;
using System.Security.Cryptography;
using System.Text;

namespace SRC.Common
{
    public static class Utility
    {
        static readonly DateTime GameDayEpoch = new(2021, 3, 29, 22, 0, 0);

        public static System.Random SeededRandom(object seed)
        {
            using var algo = SHA1.Create();
            var hash = BitConverter.ToInt32(algo.ComputeHash(Encoding.UTF8.GetBytes(seed.ToString())));
            return new(hash);
        }

        public static int GetGameDayNumber() => GetGameDayNumber(DateTime.UtcNow);
        public static int GetGameDayNumber(DateTime dt) => (int)(dt - GameDayEpoch).TotalDays;


        public static void Ternary(bool value, Action trueAction, Action falseAction)
        {
            if (value)
            {
                trueAction.Invoke();
            }
            else
            {
                falseAction.Invoke();
            }
        }
    }
}
