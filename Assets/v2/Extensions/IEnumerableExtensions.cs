using System.Collections.Generic;
using System.Numerics;

namespace GM
{
    public static class IEnumerableExtensions
    {
        public static BigDouble Sum(this IEnumerable<BigDouble> source)
        {
            BigDouble total = 0;

            foreach (BigDouble val in source)
            {
                total += val;
            }

            return total;
        }

        public static BigInteger Sum(this IEnumerable<BigInteger> source)
        {
            BigInteger total = 0;

            foreach (BigInteger val in source)
                total += val;

            return total;
        }
    }
}
