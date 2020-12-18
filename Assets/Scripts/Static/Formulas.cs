using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class Formulas
{
    // =====

    public static double CalcEnemyHealth(int stage)
    {
        return 15 * Mathf.Pow(1.5f, Mathf.Min(stage - 1, 50)) * Mathf.Pow(1.25f, Mathf.Max(stage - 50, 0));
    }

    public static double CalcBossHealth(int stage)
    {
        return CalcEnemyHealth(stage) * (stage % 5 == 0 ? 7.5f : 5.0f);
    }

    // =====

    public static double CalcEnemyGold(int stage)
    {
        return 15.0f * CalcEnemyHealth(stage) * (0.005 + (0.0002 * Mathf.Max(0, 50 - (stage - 1))));
    }

    public static double CalcBossGold(int stage)
    {
        return CalcBossHealth(stage) * 2.5f;
    }

    // ===

    public static double CalcHeroDamage(HeroID heroId)
    {
        HeroState state = GameState.GetHeroState(heroId);

        double baseCost = 3.0f;

        return baseCost * Mathf.Pow(1.11f, state.level - 1) * Mathf.Max(0.15f, 1.5f - (0.1f * (state.level - 1) / 25));
    }

    // ===

    public static double CalcHeroLevelUpCost(HeroID heroId)
    {
        HeroState state = GameState.GetHeroState(heroId);

        double baseCost = 3.0f;

        return baseCost * Mathf.Pow(1.12f, state.level - 1) * Mathf.Pow(1.05f, (state.level - 1) / 50);
    }
}