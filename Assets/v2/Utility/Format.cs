using System.Collections.Generic;
using System.Numerics;
using BonusType = GM.Common.Enums.BonusType;

namespace GM
{
    public class Format : Common.LazySingleton<Format>
    {
        public readonly static Dictionary<int, string> UnitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" }, { 5, "Q" } };

        Common.TTLCache cache = new Common.TTLCache();

        public static string Percentage(BigDouble val) => Number(val * 100) + "%";
        public static string Number(double val) => Number(new BigDouble(val));
        public static string Number(long val) => Number(new BigInteger(val));

        #region BigDouble (Cached)
        public static string Number(BigDouble val)
        {
            return Instance.cache.Get<string>($"Number/BigDouble/{val}", 60, () =>
            {
                BigDouble absVal = BigDouble.Abs(val);

                if (absVal < 1_000)
                {
                    return val.ToString("F2");
                }
                else if (BigDouble.Log(absVal, 1000) < UnitsTable.Count)
                {
                    return Number_Units(val);
                }
                else
                {
                    return Number_Exponent(val);
                }
            });
        }

        static string Number_Units(BigDouble val)
        {
            int n = (int)BigDouble.Log(val, 1000);

            BigDouble m = val / BigDouble.Pow(1000, n);

            return $"{(val < 0 ? "-" : string.Empty)}{m.ToString("F2") + UnitsTable[n]}";
        }
        static string Number_Exponent(BigDouble val) => $"{(val < 0 ? "-" : string.Empty)}{val.ToString("E2").Replace("+", "").Replace("E", "e")}";
        #endregion

        #region BigInteger (Cached)
        public static string Number(BigInteger val)
        {
            return Instance.cache.Get<string>($"Number/BigInteger/{val}", 60, () =>
            {
                BigInteger absVal = BigInteger.Abs(val);

                if (absVal < 1_000)
                {
                    return val.ToString();
                }
                else if (BigInteger.Log(absVal, 1000) < UnitsTable.Count)
                {
                    return Number_Units(val);
                }
                else
                {
                    return Number_Exponent(val);
                }
            });
        }
        static string Number_Exponent(BigInteger val) => $"{(val < 0 ? "-" : string.Empty)}{val.ToString("E2").Replace("+", "").Replace("E", "e")}";
        static string Number_Units(BigInteger val)
        {
            BigInteger absVal = BigInteger.Abs(val);

            int n = (int)BigInteger.Log(absVal, 1000);

            BigDouble m = absVal.ToBigDouble() / BigInteger.Pow(1000, n).ToBigDouble();

            return $"{(val < 0 ? "-" : string.Empty)}{m.ToString("F2") + UnitsTable[n]}";
        }
        #endregion

        public static string Bonus(BonusType bonusType, double value)
        {
            string valueString = bonusType switch
            {
                _ => Percentage(value)
            };

            return $"{valueString} {Bonus(bonusType)}";
        }

        public static string Bonus(BonusType type)
        {
            return type switch
            {
                BonusType.FLAT_CRIT_CHANCE => "Critical Hit Chance",
                BonusType.FLAT_CRIT_DMG => "Critical Hit Damage",
                BonusType.MULTIPLY_PRESTIGE_BONUS => "Runestones Bonus",
                BonusType.MERC_DAMAGE => "Merc Damage",
                BonusType.MELEE_DAMAGE => "Melee Damage",
                BonusType.RANGED_DAMAGE => "Ranged Damage",
                BonusType.ENEMY_GOLD => "Enemy Gold",
                BonusType.BOSS_GOLD => "Boss Gold",
                BonusType.ALL_GOLD => "All Gold",
                _ => type.ToString(),
            };
        }
    }
}