using System.Numerics;

using UnityEngine;

using BountyID      = BountyData.BountyID;

using PrestigeItemsData;
using CharacterData;


public static class Formulas
{
    /*
     * ===
     * This classs stores all the game formulas *BUT* they should only be called from their respective cache/manager class
     * ===
     */

    // === Bounties ===

    public static int CalcBountyHourlyIncome(BountyID bounty)
    {
        var scriptable = StaticData.Bounties.Get(bounty);

        var state = GameState.Bounties.GetState(bounty);

        return scriptable.bountyPoints + (state.level - 1);
    }

    public static double CalcWeaponDamageMultiplier(int weaponIndex, int owned)
    {
        var staticData = StaticData.Weapons.GetWeaponAtIndex(weaponIndex);

        return 1 + ((staticData.damageBonus - 1) * owned);
    }

    public static BigDouble CalcEnemyHealth(int stage)
    {
        BigDouble x = BigDouble.Pow(1.35, Mathf.Min(stage - 1, 65));
        BigDouble y = BigDouble.Pow(1.15, BigDouble.Parse(Mathf.Max(stage - 65, 0).ToString()));

        return 15 * x * y;
    }

    public static BigDouble CalcBossHealth(int stage)
    {
        return CalcEnemyHealth(stage) * 3.0f;
    }

    // =====

    public static BigDouble CalcEnemyGold(int stage)
    {
        return 10.0f * CalcEnemyHealth(stage) * (0.008 + (0.0002 * Mathf.Max(0, 100 - (stage - 1))));
    }

    public static BigDouble CalcBossGold(int stage)
    {
        return CalcEnemyGold(stage) * 5.0f;
    }

    // ===

    public static BigDouble CalcCharacterDamage(CharacterID chara)
    {
        var state           = GameState.Characters.Get(chara);
        CharacterSO data    = StaticData.Chars.Get(chara);

        return (data.purchaseCost / (10.0f + data.unlockOrder)) * state.level * BigDouble.Pow(2.0f, (state.level - 1) / 100.0f) * (1 - (0.03f * data.unlockOrder));
    }

    // ===

    public static BigDouble CalcCharacterLevelUpCost(CharacterID chara, int levels)
    {
        var state           = GameState.Characters.Get(chara);
        CharacterSO data    = StaticData.Chars.Get(chara);

        return BigMath.SumGeometricSeries(levels, data.purchaseCost, 1.075, state.level);
    }

    public static int AffordCharacterLevels(CharacterID chara)
    {
        UpgradeState state  = GameState.Characters.Get(chara);
        CharacterSO data    = StaticData.Chars.Get(chara);

        BigDouble val = BigMath.AffordGeometricSeries(GameState.Player.gold, data.purchaseCost, 1.075, state.level);

        int maxLevels = int.Parse(val.ToString());

        return Mathf.Min(StaticData.MAX_CHAR_LEVEL - state.level, maxLevels);
    }


    // ===

    public static BigDouble CalcTapDamage()
    {
        UpgradeState state = GameState.Upgrades.GetUpgrade(UpgradeID.CLICK_DAMAGE);

        return state.level * BigDouble.Pow(2.0f, (state.level - 1) / 50.0f);
    }

    // === Tap Damage Player Upgrade ===

    public static BigDouble CalcTapDamageLevelUpCost(int levels)
    {
        UpgradeState state = GameState.Upgrades.GetUpgrade(UpgradeID.CLICK_DAMAGE);

        return BigMath.SumGeometricSeries(levels, 10.0f, 1.09f, (state.level - 1));
    }

    public static int AffordTapDamageLevels()
    {
        UpgradeState state = GameState.Upgrades.GetUpgrade(UpgradeID.CLICK_DAMAGE);

        int maxLevels = int.Parse(BigMath.AffordGeometricSeries(GameState.Player.gold, 10.0, 1.09, state.level - 1).ToString());

        return Mathf.Min(StaticData.MAX_TAP_UPGRADE_LEVEL - state.level, maxLevels);
    }

    // === Prestige Points ===

    public static BigInteger CalcPrestigePoints(int stage)
    {
        if (stage < StageState.MIN_PRESTIGE_STAGE)
            return 0;

        BigDouble big = BigDouble.Pow(Mathf.CeilToInt((stage - StageState.MIN_PRESTIGE_STAGE) / 10.0f), 2.2);

        return BigInteger.Parse(big.Ceiling().ToString("F0"));
    }

    // ===

    public static BigInteger CalcNextPrestigeItemCost(int numRelics)
    {
        return BigInteger.Parse(((numRelics + 1) * BigDouble.Pow(1.5, numRelics).Floor()).ToString("F0"));
    }

    // ===

    public static double CalcPrestigeItemEffect(PrestigeItemID relic)
    {
        PrestigeItemSO data        = StaticData.PrestigeItems.Get(relic);
        UpgradeState state  = GameState.PrestigeItems.Get(relic);

        return data.baseEffect + (data.levelEffect * (state.level - 1));
    }

    // === Relics ===

    public static BigInteger CalcPrestigeItemLevelUpCost(PrestigeItemID relic, int levels)
    {
        // https://math.stackexchange.com/questions/82588/is-there-a-formula-for-sums-of-consecutive-powers-where-the-powers-are-non-inte

        PrestigeItemSO data        = StaticData.PrestigeItems.Get(relic);
        UpgradeState state  = GameState.PrestigeItems.Get(relic);

        return (data.costCoeff * SumNonIntegerPowerSeq(state.level, levels, data.costExpo)).ToBigInteger();
    }

    // === General ===

    public static BigDouble SumNonIntegerPowerSeq(int level, int levelsBuying, float s)
    {
        BigDouble Predicate(int startValue)
        {
            BigDouble x = Mathf.Pow(startValue, s + 1) / (s + 1);
            BigDouble y = Mathf.Pow(startValue, s) / 2;
            BigDouble z = Mathf.Sqrt(Mathf.Pow(startValue, s - 1));

            return x + y + z;
        }

        return Predicate(level + levelsBuying) - Predicate(level);
    }
}