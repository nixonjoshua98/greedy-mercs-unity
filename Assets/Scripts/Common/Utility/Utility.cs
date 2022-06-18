using System;
using System.Security.Cryptography;
using System.Text;

namespace GM.Common
{
    public static class Utility
    {
        public static System.Random SeededRandom(object seed)
        {
            using var algo = SHA1.Create();
            var hash = BitConverter.ToInt32(algo.ComputeHash(Encoding.UTF8.GetBytes(seed.ToString())));
            return new(hash);
        }
    }
}
