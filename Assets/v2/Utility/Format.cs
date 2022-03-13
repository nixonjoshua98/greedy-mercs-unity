using GM.Common.Enums;
using System.Collections.Generic;
using System.Numerics;

namespace GM
{
    public static class Format
    {
        public static readonly Dictionary<int, string> UnitsTable = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" }, { 5, "Q" } };

        static Common.TTLCache FormatCache = new Common.TTLCache();

        public static string Percentage(BigDouble val, int dp = 2) => $"{Number(val * 100, dp)}%";

        public static string Bonus(BonusType bonusType, BigDouble value) => $"{Number(value, bonusType)} {Bonus(bonusType)}";
        public static string Bonus(BonusType bonusType, BigDouble value, string colour) => $"<color={colour}>{Number(value, bonusType)}</color> {Bonus(bonusType)}";
        public static string Bonus(BonusType type)
        {
            return type switch
            {
                BonusType.FLAT_CRIT_CHANCE => "Critical Chance",
                BonusType.MULTIPLY_CRIT_DMG => "Critical Damage",
                BonusType.MULTIPLY_PRESTIGE_BONUS => "Runestones Bonus",
                BonusType.MULTIPLY_MERC_DMG => "Merc Damage",
                BonusType.MULTIPLY_ALL_DMG => "All Damage",
                BonusType.MULTIPLY_MELEE_DMG => "Melee Damage",
                BonusType.MULTIPLY_RANGED_DMG => "Ranged Damage",
                BonusType.MULTIPLY_ENEMY_GOLD => "Enemy Gold",
                BonusType.MULTIPLY_BOSS_GOLD => "Boss Gold",
                BonusType.MULTIPLY_ALL_GOLD => "All Gold",
                BonusType.MULTIPLY_TAP_DMG => "Tap Damage",
                _ => type.ToString(),
            };
        }

        public static string Number(BigDouble val, BonusType bonus)
        {
            return bonus switch
            {
                BonusType.FLAT_TAP_DMG => Number(val),
                _ => Percentage(val.ToDouble())
            };
        }

        public static string Number(long val) => Number(new BigInteger(val));

        public static string Number(BigDouble val, int dp = 2)
        {
            return FormatCache.Get<string>($"Number/BigDouble/{val}", 60, () =>
            {
                BigDouble absVal = BigDouble.Abs(val);

                if (absVal < 1_000)
                    return val.ToString($"F{dp}");

                else if (BigDouble.Log(absVal, 1000) < UnitsTable.Count)
                    return BigDouble_Units(val, dp);

                else
                    return BigDouble_Exponent(val);
            });
        }

        public static string Number(BigInteger val)
        {
            return FormatCache.Get<string>($"Number/BigInteger/{val}", 60, () =>
            {
                BigInteger absVal = BigInteger.Abs(val);

                if (absVal < 1_000)
                    return val.ToString();

                else if (BigInteger.Log(absVal, 1000) < UnitsTable.Count)
                    return BigInteger_Units(val);

                else
                    return BigInteger_Exponent(val);
            });
        }

        static string BigInteger_Exponent(BigInteger val)
        {
            return $"{(val < 0 ? "-" : string.Empty)}{val.ToString("E2").Replace("+", "").Replace("E", "e")}";
        }

        static string BigInteger_Units(BigInteger val)
        {
            BigInteger absVal = BigInteger.Abs(val);

            int n = (int)BigInteger.Log(absVal, 1000);

            BigDouble m = absVal.ToBigDouble() / BigInteger.Pow(1000, n).ToBigDouble();

            return $"{(val < 0 ? "-" : string.Empty)}{m.ToString("F2") + UnitsTable[n]}";
        }

        static string BigDouble_Units(BigDouble val, int dp = 2)
        {
            BigDouble absVal = BigDouble.Abs(val);

            int n = (int)BigDouble.Log(absVal, 1000);

            BigDouble m = absVal / BigDouble.Pow(1000, n);

            return $"{(val < 0 ? "-" : string.Empty)}{m.ToString($"F{dp}") + UnitsTable[n]}";
        }

        static string BigDouble_Exponent(BigDouble val)
        {
            return $"{(val < 0 ? "-" : string.Empty)}{val.ToString("E2").Replace("+", "").Replace("E", "e")}";
        }
    }
}
