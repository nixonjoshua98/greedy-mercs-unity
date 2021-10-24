using System.Collections.Generic;
using System.Numerics;

// I think this could benefit from being converted to a lazy loaded singleton (non monobehaviour) and using a TTLCache with a long timer

public static class Format
{
    readonly static Dictionary<int, string> units = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" }, { 5, "Q" } };

    public static string Percentage(BigDouble val) => Number(val * 100) + "%";
    public static string Number(double val) => Number(new BigDouble(val));
    public static string Number(long val) => Number(new BigInteger(val));

    public static string Number(BigDouble val)
    {
        if (BigDouble.Abs(val) < 1_000)
            return val.ToString("F2");

        int n = (int)BigDouble.Log(val, 1000);

        // Show M, T, Q etc...
        if (n < units.Count)
        {
            BigDouble m = val / BigDouble.Pow(1000.0f, n);

            return string.Format("{0}{1}", m.ToString("F2"), units[n]);
        }

        // Fall back to exponent view
        return val.ToString("E2").Replace("+", "").Replace("E", "e");
    }

    public static string Number(BigInteger val)
    {
        if (BigInteger.Abs(val) < 1_000)
            return val.ToString();

        int n = (int)BigInteger.Log(val, 1000);

        // Show M, T, Q etc...
        if (n < units.Count)
        {
            BigDouble m = val.ToBigDouble() / BigInteger.Pow(1000, n).ToBigDouble();

            return m.ToString("F2") + units[n];
        }

        // Fall back to exponent view
        return val.ToString("E2").Replace("+", "").Replace("E", "e");
    }

    public static string Bonus(BonusType bonusType, double value)
    {
        return bonusType switch
        {
            BonusType.FLAT_ENERGY_CAPACITY => $"{value} Energy Capacity",
            BonusType.FLAT_ENERGY_INCOME => $"{Number(value)} Energy per Minute",
            BonusType.FLAT_CRIT_CHANCE => $"{Number(value * 100)}% Critical Hit Chance",
            BonusType.FLAT_CRIT_DMG => $"{Number(value * 100)}% Critical Hit Damage",
            BonusType.MULTIPLY_PRESTIGE_BONUS => $"{Number(value * 100)}% Runestones",
            BonusType.TAP_DAMAGE => $"{Number(value * 100)}% Tap Damage",
            BonusType.MERC_DAMAGE => $"{Number(value * 100)}% Merc Damage",
            BonusType.MELEE_DAMAGE => $"{Number(value * 100)}% Melee Damage",
            BonusType.RANGED_DAMAGE => $"{Number(value * 100)}% Ranged Damage",
            BonusType.ENEMY_GOLD => $"{Number(value * 100)}% Enemy Gold",
            BonusType.BOSS_GOLD => $"{Number(value * 100)}% Boss Gold",
            BonusType.ALL_GOLD => $"{Number(value * 100)}% All Gold",
            BonusType.CHAR_TAP_DAMAGE_ADD => $"{Number(value * 100)}% Damage From Merc",
            _ => $"{Number(value)} {bonusType}",
        };
    }
}