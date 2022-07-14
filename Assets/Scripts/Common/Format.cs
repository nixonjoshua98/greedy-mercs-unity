using GM.Enums;
using System.Numerics;

namespace GM
{
    public static class Format
    {
        private static readonly Common.TTLCache FormatCache = new();

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
                Enums.BonusType.FLAT_CRIT_CHANCE => "Crit Chance",
                Enums.BonusType.MULTIPLY_CRIT_DMG => "Crit Damage",
                Enums.BonusType.MULTIPLY_PRESTIGE_BONUS => "Runestones Bonus",
                Enums.BonusType.MULTIPLY_MERC_DMG => "Merc Damage",
                Enums.BonusType.MULTIPLY_ALL_DMG => "All Damage",
                Enums.BonusType.MULTIPLY_MELEE_DMG => "Melee Damage",
                Enums.BonusType.MULTIPLY_RANGED_DMG => "Ranged Damage",
                Enums.BonusType.MULTIPLY_ENEMY_GOLD => "Enemy Gold",
                Enums.BonusType.MULTIPLY_BOSS_GOLD => "Boss Gold",
                Enums.BonusType.MULTIPLY_ALL_GOLD => "All Gold",
                Enums.BonusType.MULTIPLY_TAP_DMG => "Tap Damage",
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

        public static string Number(float val, BonusType bonus)
        {
            return Number(new BigDouble(val), bonus);
        }

        public static string Number(long val)
        {
            return Number(new BigInteger(val));
        }

        public static string Number(BigDouble val, int dp = 2)
        {
            return FormatCache.Get<string>($"BigDouble/{val}", 60, () =>
            {
                if (BigDouble.Abs(val) < 1_000)
                    return val.ToString($"F{dp}");

                return val.ToString(StringFormat.Units);
            });
        }

        public static string Number(BigInteger val)
        {
            return FormatCache.Get<string>($"BigInteger/{val}", 60, () =>
            {
                if (BigInteger.Abs(val) < 1_000)
                    return val.ToString();

                return val.ToString(StringFormat.Units);
            });
        }
    }
}
