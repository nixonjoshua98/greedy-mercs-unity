using System.Numerics;

using UnityEngine;

using BountyID      = BountyData.BountyID;

using RelicData;
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

        BigDouble big = BigDouble.Pow(Mathf.CeilToInt((stage - StageState.MIN_PRESTIGE_STAGE) / 10.0f), 2.1);

        return BigInteger.Parse(big.Ceiling().ToString("F0"));
    }

    // ===

    public static BigInteger CalcNextRelicCost(int numRelics)
    {
        return BigInteger.Parse(BigDouble.Pow(1.5, numRelics).Floor().ToString("F0"));
    }

    // ===

    public static double CalcRelicEffect(RelicID relic)
    {
        RelicSO data        = StaticData.Relics.Get(relic);
        UpgradeState state  = GameState.Relics.Get(relic);

        return data.baseEffect + (data.levelEffect * (state.level - 1));
    }

    // === Relics ===

    public static BigInteger CalcRelicLevelUpCost(RelicID relic, int levels)
    {
        RelicSO data        = StaticData.Relics.Get(relic);
        UpgradeState state  = GameState.Relics.Get(relic);

        BigDouble val = BigMath.SumGeometricSeries(levels, data.baseCost, data.costPower, state.level - 1);

        return BigInteger.Parse(val.Ceiling().ToString("F0"));
    }

    public static int AffordRelicLevels(RelicID relic)
    {
        UpgradeState state  = GameState.Relics.Get(relic);
        RelicSO data        = StaticData.Relics.Get(relic);

        BigDouble bigAnswer = BigMath.AffordGeometricSeries(GameState.Player.prestigePoints.ToBigDouble(), data.baseCost, data.costPower, state.level - 1);

        int maxLevels = int.Parse(bigAnswer.ToString());

        return Mathf.Min(data.maxLevel - state.level, maxLevels);
    }
}