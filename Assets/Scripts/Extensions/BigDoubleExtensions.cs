using System.Collections.Generic;

namespace GM
{
    public static class BigDoubleExtensions
    {
        private static readonly Dictionary<int, string> UnitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" }, { 5, "Q" } };

        public static string ToString(this BigDouble value, Enums.StringFormat format)
        {
            return format switch
            {
                Enums.StringFormat.Exponent => ExponentFormat(value),
                Enums.StringFormat.Units => UnitsFormat(value),
                _ => value.ToString(),
            };
        }

        static string ExponentFormat(BigDouble value)
        {
            string sign = value < 0 ? "-" : string.Empty;

            return $"{sign}{value.ToString("E2").Replace("+", "").Replace("E", "e")}";
        }

        static string UnitsFormat(BigDouble value)
        {
            BigDouble absVal = BigDouble.Abs(value);

            long n = (long)BigDouble.Log(absVal, 1000);

            if (n >= UnitsTable.Count)
                return value.ToString(Enums.StringFormat.Exponent);

            BigDouble m = absVal / BigDouble.Pow(1000, n);

            string sign = value < 0 ? "-" : string.Empty;

            return $"{sign}{m.ToString($"F2")}{UnitsTable[(int)n]}";
        }
    }
}
