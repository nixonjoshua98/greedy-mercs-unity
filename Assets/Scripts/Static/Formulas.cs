using System.Numerics;

using UnityEngine;

public static class Formulas
{
    /*
     * ===
     * This classs stores all the game formulas *BUT* they should only be called from their respective cache/manager class
     * ===
     */

    // =====

    public static BigDouble CalcEnemyHealth(int stage)
    {
        return 15.0 * BigDouble.Pow(1.3f, Mathf.Min(stage - 1, 70)) * BigDouble.Pow(1.15f, Mathf.Max(stage - 70, 0));
    }

    public static BigDouble CalcBossHealth(int stage)
    {
        return CalcEnemyHealth(stage) * 3.0f;
    }

    // =====

    public static BigDouble CalcEnemyGold(int stage)
    {
        return 12.5f * CalcEnemyHealth(stage) * (0.005 + (0.0002 * Mathf.Max(0, 50 - (stage - 1))));
    }

    public static BigDouble CalcBossGold(int stage)
    {
        return CalcEnemyGold(stage) * 5.0f;
    }

    // ===

    public static BigDouble CalcCharacterDamage(CharacterID heroId)
    {
        var state = GameState.Characters.GetCharacter(heroId);

        CharacterStaticData hero = CharacterResources.GetCharacter(heroId);

        return (hero.PurchaseCost / 10.0f) * state.level * BigDouble.Pow(4.0f, (state.level - 1) / 100.0f);
    }

    // ===

    public static BigDouble CalcCharacterLevelUpCost(CharacterID heroId, int levels)
    {
        var state = GameState.Characters.GetCharacter(heroId);

        CharacterStaticData hero = CharacterResources.GetCharacter(heroId);

        return BigMath.SumGeometricSeries(levels, hero.PurchaseCost, 1.075, state.level);
    }

    public static int AffordCharacterLevels(CharacterID charaId)
    {
        UpgradeState state              = GameState.Characters.GetCharacter(charaId);
        CharacterStaticData staticData  = CharacterResources.GetCharacter(charaId);

        return int.Parse(BigMath.AffordGeometricSeries(GameState.Player.gold, staticData.PurchaseCost, 1.075, state.level).ToString());
    }


    // ===

    public static BigDouble CalcTapDamage()
    {
        UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(UpgradeID.TAP_DAMAGE);

        return state.level * BigDouble.Pow(2.0f, (state.level - 1) / 35.0f);
    }

    // === Tap Damage Player Upgrade ===

    public static BigDouble CalcTapDamageLevelUpCost(int levels)
    {
        UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(UpgradeID.TAP_DAMAGE);

        return BigMath.SumGeometricSeries(levels, 10.0f, 1.09f, state.level);
    }

    public static int AffordTapDamageLevels()
    {
        UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(UpgradeID.TAP_DAMAGE);

        return int.Parse(BigMath.AffordGeometricSeries(GameState.Player.gold, 10.0, 1.09, state.level).ToString());
    }

    // === Prestige Points ===

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

    // === Relics ===

    public static BigInteger CalcRelicLevelUpCost(RelicID relic, int levels)
    {
        RelicStaticData staticData = StaticData.GetRelic(relic);

        UpgradeState state = GameState.Relics.GetRelic(relic);

        BigDouble val = BigMath.SumGeometricSeries(levels, staticData.baseCost, staticData.costPower, state.level);

        return BigInteger.Parse(val.Ceiling().ToString("F0"));
    }

    public static int AffordRelicLevels(RelicID relic)
    {
        UpgradeState state          = GameState.Relics.GetRelic(relic);
        RelicStaticData staticData  = StaticData.GetRelic(relic);

        return int.Parse(BigMath.AffordGeometricSeries(GameState.Player.prestigePoints.AsBigDouble(), staticData.baseCost, staticData.costPower, state.level).ToString());
    }
}