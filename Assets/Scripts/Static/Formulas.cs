using System.Numerics;

using UnityEngine;

using RelicID           = RelicData.RelicID;
using CharacterID       = CharacterData.CharacterID;
using RelicStaticData   = RelicData.RelicStaticData;

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
        return 15.0 * BigDouble.Pow(1.29f, Mathf.Min(stage - 1, 65)) * BigDouble.Pow(1.15f, Mathf.Max(stage - 65, 0));
    }

    public static BigDouble CalcBossHealth(int stage)
    {
        return CalcEnemyHealth(stage) * 3.0f;
    }

    // =====

    public static BigDouble CalcEnemyGold(int stage)
    {
        return 12.5f * CalcEnemyHealth(stage) * (0.0045 + (0.0002 * Mathf.Max(0, 65 - (stage - 1))));
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

        return (hero.PurchaseCost / 10.0f) * state.level * BigDouble.Pow(3.5f, (state.level - 1) / 100.0f) * (1 - (0.032f * (int)heroId));
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

        BigDouble bigAnswer = BigMath.AffordGeometricSeries(GameState.Player.gold, staticData.PurchaseCost, 1.075, state.level);

        int maxLevels = int.Parse(bigAnswer.ToString());

        return Mathf.Min(StaticData.MAX_CHAR_LEVEL - state.level, maxLevels);
    }


    // ===

    public static BigDouble CalcTapDamage()
    {
        UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(UpgradeID.TAP_DAMAGE);

        return state.level * BigDouble.Pow(2.0f, (state.level - 1) / 50.0f);
    }

    // === Tap Damage Player Upgrade ===

    public static BigDouble CalcTapDamageLevelUpCost(int levels)
    {
        UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(UpgradeID.TAP_DAMAGE);

        return BigMath.SumGeometricSeries(levels, 10.0f, 1.09f, (state.level - 1));
    }

    public static int AffordTapDamageLevels()
    {
        UpgradeState state = GameState.PlayerUpgrades.GetUpgrade(UpgradeID.TAP_DAMAGE);

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
        RelicStaticData staticData = StaticData.Relics.Get(relic);

        UpgradeState state = GameState.Relics.GetRelic(relic);

        return staticData.baseEffect + (staticData.levelEffect * (state.level - 1));
    }

    // === Relics ===

    public static BigInteger CalcRelicLevelUpCost(RelicID relic, int levels)
    {
        RelicStaticData staticData = StaticData.Relics.Get(relic);

        UpgradeState state = GameState.Relics.GetRelic(relic);

        BigDouble val = BigMath.SumGeometricSeries(levels, staticData.baseCost, staticData.costPower, state.level - 1);

        return BigInteger.Parse(val.Ceiling().ToString("F0"));
    }

    public static int AffordRelicLevels(RelicID relic)
    {
        UpgradeState state          = GameState.Relics.GetRelic(relic);
        RelicStaticData staticData  = StaticData.Relics.Get(relic);

        BigDouble bigAnswer = BigMath.AffordGeometricSeries(GameState.Player.prestigePoints.AsBigDouble(), staticData.baseCost, staticData.costPower, state.level - 1);

        int maxLevels = int.Parse(bigAnswer.ToString());

        return Mathf.Min(staticData.maxLevel - state.level, maxLevels);
    }
}