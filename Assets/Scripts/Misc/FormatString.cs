
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


    #region Seconds
    public static string Seconds(double seconds) => Seconds((long)seconds);
    public static string Seconds(long seconds)
    {
        long hours = seconds / 3_600;
        seconds -= (3_600 * hours);

        long mins = seconds / 60;
        seconds -= (60 * mins);

        return string.Format("{0}h {1}m {2}s", hours.ToString().PadLeft(2, '0'), mins.ToString().PadLeft(2, '0'), seconds.ToString().PadLeft(2, '0'));
    }
    #endregion


    // BonusType
    public static string Bonus(BonusType bonusType, double value)
    {
        switch (bonusType)
        {
            case BonusType.FLAT_ENERGY_CAPACITY:    return $"{value} Energy Capacity";
            case BonusType.FLAT_ENERGY_INCOME:      return $"{Number(value)} Energy per Minute";
            case BonusType.FLAT_CRIT_CHANCE:        return $"{Number(value * 100)}% Critical Hit Chance";
            case BonusType.FLAT_CRIT_DMG:      return $"{Number(value * 100)}% Critical Hit Damage";
            case BonusType.MULTIPLY_PRESTIGE_BONUS:  return $"{Number(value * 100)}% Runestones";
            case BonusType.TAP_DAMAGE:      return $"{Number(value * 100)}% Tap Damage";
            case BonusType.MERC_DAMAGE:     return $"{Number(value * 100)}% Merc Damage";
            case BonusType.MELEE_DAMAGE:    return $"{Number(value * 100)}% Melee Damage";
            case BonusType.RANGED_DAMAGE:   return $"{Number(value * 100)}% Ranged Damage";
            case BonusType.ENEMY_GOLD:  return $"{Number(value * 100)}% Enemy Gold";
            case BonusType.BOSS_GOLD:   return $"{Number(value * 100)}% Boss Gold";
            case BonusType.ALL_GOLD:    return $"{Number(value * 100)}% All Gold";
            case BonusType.CHAR_TAP_DAMAGE_ADD: return $"{Number(value * 100)}% Damage From Merc";

            default: 
                return $"{Number(value)} <Missing>";
        }
    }
}