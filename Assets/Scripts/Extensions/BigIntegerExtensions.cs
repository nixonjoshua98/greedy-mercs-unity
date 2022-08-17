using System.Collections.Generic;
using System.Numerics;

namespace SRC
{
    public static class BigIntegerExtensions
    {
        private static readonly Dictionary<int, string> UnitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" }, { 5, "Q" } };

        public static BigDouble ToBigDouble(this BigInteger source)
        {
            return BigDouble.Parse(source.ToString());
        }

        private static string ExponentFormat(BigInteger value)
        {
            string sign = value < 0 ? "-" : string.Empty;

            return $"{sign}{value.ToString("E2").Replace("+", "").Replace("E", "e")}";
        }

        private static string UnitsFormat(BigInteger value)
        {
            BigInteger absVal = BigInteger.Abs(value);

            int n = (int)BigInteger.Log(absVal, 1000);

            if (n >= UnitsTable.Count)
                return ExponentFormat(value);

            BigDouble m = absVal.ToBigDouble() / BigInteger.Pow(1000, n).ToBigDouble();

            string sign = value < 0 ? "-" : string.Empty;

            return $"{sign}{m:F2}{UnitsTable[n]}";
        }
    }
}
