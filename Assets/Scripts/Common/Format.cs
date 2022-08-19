using SRC.Common.Enums;
using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace SRC
{
    public static class Format
    {
        public static string Colour(Color col, object obj) => $"<color={col.ToHex()}>{obj}</color>";

        public static string ShortTimeSpan(TimeSpan ts)
        {
            List<string> parts = new();

            if (ts.Hours > 0)
                parts.Add($"{ts.Hours}h");

            if (ts.Minutes > 0)
                parts.Add($"{ts.Minutes}m");

            if (parts.Count == 0 && ts.Seconds > 0)
                parts.Add($"{ts.Seconds}s");

            return string.Join(" ", parts);
        }

        public static string Number(BigInteger val) => val.Format();

        public static string Number(long val) => Number(new BigInteger(val));











        /* Big Bigdouble */

        public static string Number(BigDouble val, int decimalPlaces = 2)
        {
            return val.Format(decimalPlaces);
        }

        public static string Percentage(BigDouble val, int dp = 2)
        {
            return $"{Number(val * 100, dp)}%";
        }

        public static string Bonus(BonusType bonusType, BigDouble value)
        {
            return $"{Number(value, bonusType)} {BonusType(bonusType)}";
        }

        public static string BonusValue(BonusType bonusType, BigDouble value, string colour = "orange")
        {
            return $"<color={colour}>{Number(value, bonusType)}</color> {BonusType(bonusType)}";
        }

        public static string BonusType(BonusType type)
        {
            return type switch
            {
                SRC.Common.Enums.BonusType.FLAT_CRIT_CHANCE => "Crit Chance",
                SRC.Common.Enums.BonusType.MULTIPLY_CRIT_DMG => "Crit Damage",
                SRC.Common.Enums.BonusType.MULTIPLY_PRESTIGE_BONUS => "Runestones Bonus",
                SRC.Common.Enums.BonusType.MULTIPLY_MERC_DMG => "Merc Damage",
                SRC.Common.Enums.BonusType.MULTIPLY_ALL_DMG => "All Damage",
                SRC.Common.Enums.BonusType.MULTIPLY_MELEE_DMG => "Melee Damage",
                SRC.Common.Enums.BonusType.MULTIPLY_RANGED_DMG => "Ranged Damage",
                SRC.Common.Enums.BonusType.MULTIPLY_ENEMY_GOLD => "Enemy Gold",
                SRC.Common.Enums.BonusType.MULTIPLY_BOSS_GOLD => "Boss Gold",
                SRC.Common.Enums.BonusType.MULTIPLY_ALL_GOLD => "All Gold",
                SRC.Common.Enums.BonusType.MULTIPLY_TAP_DMG => "Tap Damage",
                _ => type.ToString(),
            };
        }

        public static string Number(BigDouble val, BonusType bonus)
        {
            return bonus switch
            {
                _ => Percentage(val.ToDouble())
            };
        }

        public static string Number(BonusType bonus, BigDouble val)
        {
            return bonus switch
            {
                _ => Percentage(val.ToDouble())
            };
        }
    }
}
