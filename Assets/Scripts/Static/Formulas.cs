using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class Formulas
{
    // =====

    public static double CalcEnemyHealth(int stage)
    {
        return 15 * Mathf.Pow(1.25f, Mathf.Min(stage - 1, 25)) * Mathf.Pow(1.25f, Mathf.Max(stage - 25, 0));
    }

    public static double CalcBossHealth(int stage)
    {
        return CalcEnemyHealth(stage) * 3.0f;
    }

    // =====

    public static double CalcEnemyGold(int stage)
    {
        return 10.0f * CalcEnemyHealth(stage) * (0.005 + (0.0002 * Mathf.Max(0, 100 - (stage - 1))));
    }

    public static double CalcBossGold(int stage)
    {
        return CalcEnemyGold(stage) * 5.0f;
    }

    // ===

    public static double CalcHeroDamage(HeroID heroId)
    {
        HeroState state = GameState.GetHeroState(heroId);

        return CalcHeroLevelUpCost(heroId) * Mathf.Max(0.15f, 1.0f - (0.11f * (state.level - 1) / 15));
    }

    // ===

    public static double CalcHeroLevelUpCost(HeroID heroId)
    {
        HeroState state = GameState.GetHeroState(heroId);

        double baseCost = 3.0f;

        return baseCost * Mathf.Pow(1.125f, state.level - 1) * Mathf.Pow(1.1f, (state.level - 1) / 15);
    }
}