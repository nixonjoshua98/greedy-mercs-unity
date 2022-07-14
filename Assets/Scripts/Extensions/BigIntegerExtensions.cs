using System.Collections.Generic;
using System.Numerics;

namespace GM
{
    public static class BigIntegerExtensions
    {
        private static readonly Dictionary<int, string> UnitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" }, { 5, "Q" } };

        public static BigDouble ToBigDouble(this BigInteger source)
        {
            return BigDouble.Parse(source.ToString());
        }

        public static string ToString(this BigInteger value, Enums.StringFormat format)
        {
            return format switch
            {
                Enums.StringFormat.Exponent => ExponentFormat(value),
                Enums.StringFormat.Units => UnitsFormat(value),
                _ => value.ToString(),
            };
        }

        static string ExponentFormat(BigInteger value)
        {
            string sign = value < 0 ? "-" : string.Empty;

            return $"{sign}{value.ToString("E2").Replace("+", "").Replace("E", "e")}";
        }

        static string UnitsFormat(BigInteger value)
        {
            BigInteger absVal = BigInteger.Abs(value);

            int n = (int)BigInteger.Log(absVal, 1000);

            if (n >= UnitsTable.Count)
                return value.ToString(Enums.StringFormat.Exponent);

            BigDouble m = absVal.ToBigDouble() / BigInteger.Pow(1000, n).ToBigDouble();

            string sign = value < 0 ? "-" : string.Empty;

            return $"{sign}{m:F2}{UnitsTable[n]}";
        }
    }
}
