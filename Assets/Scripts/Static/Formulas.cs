using UnityEngine;

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
        var state = GameState.GetCharacter(heroId);

        CharacterStaticData hero = CharacterResources.GetCharacter(heroId);

        return (hero.PurchaseCost / 10.0f) * state.level * Mathf.Pow(4.0f, (state.level - 1) / 100.0f);
    }

    // ===

    public static double CalcHeroLevelUpCost(CharacterID heroId, int levels)
    {
        var state = GameState.GetCharacter(heroId);

        CharacterStaticData hero = CharacterResources.GetCharacter(heroId);

        return (hero.PurchaseCost * Mathf.Pow(1.075f, state.level)) * ((1 - Mathf.Pow(1.075f, levels)) / (1 - 1.075f));
    }

    // ===

    public static double CalcTapDamage()
    {
        UpgradeState state = GameState.Player.GetUpgrade(PlayerUpgradeID.TAP_DAMAGE);

        return state.level * Mathf.Pow(2.0f, (state.level - 1) / 35.0f);
    }

    // ===

    public static double CalcTapDamageLevelUpCost(int levels)
    {
        UpgradeState state = GameState.Player.GetUpgrade(PlayerUpgradeID.TAP_DAMAGE);

        return (10.0f * Mathf.Pow(1.09f, state.level - 1)) * ((1 - Mathf.Pow(1.09f, levels)) / (1 - 1.09f));
    }

    // ===

    public static ulong CalcPrestigePoints(int stage)
    {
        if (GameState.Stage.stage < StageData.MIN_PRESTIGE_STAGE)
            return 0;

        return (ulong)Mathf.CeilToInt(Mathf.Pow((stage - 75) / 14.0f, 2.0f));
    }

    // ===

    public static ulong CalcNextRelicCost(int numRelics)
    {
        return (ulong)Mathf.Pow(2.0f, numRelics);
    }
}