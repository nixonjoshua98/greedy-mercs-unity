using System;
using System.Security.Cryptography;
using System.Text;

namespace SRC.Common
{
    public static class Utility
    {
        static readonly DateTime GameDayEpoch = new(2021, 3, 29, 20, 0, 0);

        public static System.Random SeededRandom(object seed)
        {
            using var algo = SHA1.Create();
            var hash = BitConverter.ToInt32(algo.ComputeHash(Encoding.UTF8.GetBytes(seed.ToString())));
            return new(hash);
        }

        public static int GetGameDayNumber(DateTime? dt = null) => (int)((dt ?? DateTime.UtcNow) - GameDayEpoch).TotalDays;
        public static DateTime GetGameDayDate(int? gameDay = null) => GameDayEpoch + TimeSpan.FromDays(gameDay ?? GetGameDayNumber());



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
