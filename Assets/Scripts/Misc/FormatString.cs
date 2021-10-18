
using System.Collections.Generic;
using System.Numerics;


public static class FormatString
{
    readonly static Dictionary<int, string> units = new Dictionary<int, string> { { 0, "" }, { 1, "K" }, { 2, "M" }, { 3, "B" }, { 4, "T" }, { 5, "Q" } };

    #region Number
    public static string Number(BigDouble val, string prefix = "")
    {
        if (BigDouble.Abs(val) < 1) // Value is less than 1.0 so we just return it rounded
            return string.Format("{0}{1}", val.ToString("F2"), prefix);

        int n = (int)BigDouble.Log(val, 1000);

        BigDouble m = val / BigDouble.Pow(1000.0f, n);

        if (n < units.Count) // Value is within the stored units
            return string.Format("{0}{1}{2}", m.ToString("F2"), units[n], prefix);

        string toStringResult = val.ToString("E2").Replace("+", "").Replace("E", "e");

        // Value is larger than the units provded, so return a exponent/mantissa
        return string.Format("{0}{1}", toStringResult, prefix);
    }

    public static string Number(BigInteger val)
    {
        if (val < 1_000)
            return val.ToString();

        int n = (int)BigInteger.Log(val, 1000);

        BigDouble m = val.ToBigDouble() / BigInteger.Pow(1000, n).ToBigDouble();

        if (n < units.Count)
            return m.ToString("F2") + units[n];

        return val.ToString("E2").Replace("+", "").Replace("E", "e");
    }

    public static string Number(double val, string prefix = "")
    {
        return Number(new BigDouble(val), prefix: prefix);
    }
    #endregion


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