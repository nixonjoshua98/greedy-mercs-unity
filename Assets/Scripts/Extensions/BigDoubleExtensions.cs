using System.Collections.Generic;

namespace GM
{
    public static class BigDoubleExtensions
    {
        private static readonly Dictionary<int, string> UnitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" }, { 5, "Q" } };

        public static string Format(this BigDouble value, int decimalPlaces = 2)
        {
            return UnitsFormat(value, decimalPlaces);
        }

        static string ExponentFormat(BigDouble value, int decimalPlaces)
        {
            string sign = value < 0 ? "-" : string.Empty;

            return $"{sign}{value.ToString($"E{decimalPlaces}").Replace("+", "").Replace("E", "e")}";
        }

        static string UnitsFormat(BigDouble value, int decimalPlaces)
        {
            BigDouble absVal = BigDouble.Abs(value);

            // Temp fix for the issue of it returning '0.0K' instead of '0.0' (unsure of the cause)
            if (absVal < 1_000)
            {
                return value.ToString($"F{decimalPlaces}");
            }

            long n = (long)BigDouble.Log(absVal, 1000);

            // Cannot format this number with regular units
            if (n > (UnitsTable.Count - 1))
            {
                return ExponentFormat(value, decimalPlaces);
            }

            BigDouble m = absVal / BigDouble.Pow(1000, n);

            string sign = value < 0 ? "-" : string.Empty;

            return $"{sign}{m.ToString($"F{decimalPlaces}")}{UnitsTable[(int)n]}";
        }
    }
}
