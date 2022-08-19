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

        public static string Format(this BigInteger value)
        {
            int n = (int)BigInteger.Log(BigInteger.Abs(value), 1000);

            if (n >= UnitsTable.Count)
                return ToExponent(value);

            return ToUnits(value);
        }

        private static string ToExponent(this BigInteger value)
        {
            string sign = value < 0 ? "-" : string.Empty;

            return $"{sign}{value.ToString("E2").Replace("+", "").Replace("E", "e")}";
        }

        private static string ToUnits(this BigInteger value)
        {
            var absVal = BigInteger.Abs(value);

            if (absVal < 1_000)
                return value.ToString();

            int n = (int)BigInteger.Log(absVal, 1000);

            BigDouble m = absVal.ToBigDouble() / BigInteger.Pow(1000, n).ToBigDouble();

            string sign = value < 0 ? "-" : string.Empty;

            if (n == 0) // No need for decimal points
                return $"{sign}{m}{UnitsTable[n]}";

            return $"{sign}{m:F2}{UnitsTable[n]}";
        }
    }
}
