using System.Numerics;

using UnityEngine;

using BreakInfinity;

public static class Formulas
{
    /*
     * ===
     * This classs stores all the game formulas *BUT* they should only be called from their respective cache/manager class
     * ===
     */

    // =====

    public static double CalcEnemyHealth(int stage)
    {
        return 15.0 * Mathf.Pow(1.3f, Mathf.Min(stage - 1, 70)) * Mathf.Pow(1.15f, Mathf.Max(stage - 70, 0));
    }

    public static double CalcBossHealth(int stage)
    {
        return CalcEnemyHealth(stage) * 3.0f;
    }

    // =====

    public static double CalcEnemyGold(int stage)
    {
        return 12.5f * CalcEnemyHealth(stage) * (0.005 + (0.0002 * Mathf.Max(0, 50 - (stage - 1))));
    }

    public static double CalcBossGold(int stage)
    {
        return CalcEnemyGold(stage) * 5.0f;
    }

    // ===

    public static double CalcHeroDamage(CharacterID heroId)
    {
        var state = GameState.Characters.GetCharacter(heroId);

        CharacterStaticData hero = CharacterResources.GetCharacter(heroId);

        return (hero.PurchaseCost / 10.0f) * state.level * Mathf.Pow(4.0f, (state.level - 1) / 100.0f);
    }

    // ===

    public static double CalcHeroLevelUpCost(CharacterID heroId, int levels)
    {
        var state = GameState.Characters.GetCharacter(heroId);

        CharacterStaticData hero = CharacterResources.GetCharacter(heroId);

        return (hero.PurchaseCost * Mathf.Pow(1.075f, state.level)) * ((1 - Mathf.Pow(1.075f, levels)) / (1 - 1.075f));
    }

    // ===

    public static double CalcTapDamage()
    {
        UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(UpgradeID.TAP_DAMAGE);

        return state.level * Mathf.Pow(2.0f, (state.level - 1) / 35.0f);
    }

    // ===

    public static double CalcTapDamageLevelUpCost(int levels)
    {
        UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(UpgradeID.TAP_DAMAGE);

        return (10.0f * Mathf.Pow(1.09f, state.level - 1)) * ((1 - Mathf.Pow(1.09f, levels)) / (1 - 1.09f));
    }

    // ===

    public static BigInteger CalcPrestigePoints(int stage)
    {
        return stage >= StageData.MIN_PRESTIGE_STAGE ? BigInteger.Pow(Mathf.CeilToInt((stage - 70) / 10.0f), 2) : 0;
    }

    // ===

    public static BigInteger CalcNextRelicCost(int numRelics)
    {
        return BigInteger.Pow(2, numRelics);
    }

    // ===

    public static double CalcRelicEffect(RelicID relic)
    {
        RelicStaticData staticData = StaticData.GetRelic(relic);

        UpgradeState state = GameState.Relics.GetRelic(relic);

        return staticData.baseEffect + (staticData.levelEffect * (state.level - 1));
    }

    // ===

    public static BigInteger CalcRelicLevelUpCost(RelicID relic, int levels)
    {
        RelicStaticData staticData = StaticData.GetRelic(relic);

        UpgradeState state = GameState.Relics.GetRelic(relic);

        BigDouble levelCost      = BigDouble.Pow(staticData.costPower, state.level - 1);
        BigDouble theOtherCost   = (1 - BigDouble.Pow(staticData.costPower, levels));

        BigDouble val = (staticData.baseCost * levelCost * theOtherCost / (1 - staticData.costPower));

        return BigInteger.Parse(BigDouble.Ceiling(val).ToString());
    }
}