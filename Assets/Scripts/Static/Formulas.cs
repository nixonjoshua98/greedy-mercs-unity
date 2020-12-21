﻿using System.Collections;
using System.Collections.Generic;

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
        var state = GameState.GetHeroState(heroId);

        var data = ServerData.GetStaticHeroData(heroId);

        return Mathf.Pow(data.baseCost, 1.35f) * Mathf.Pow(1.13f, state.level - 1);
    }

    // ===

    public static double CalcHeroLevelUpCost(HeroID heroId)
    {
        var state = GameState.GetHeroState(heroId);

        var data = ServerData.GetStaticHeroData(heroId);

        return data.baseCost * Mathf.Pow(1.175f, state.level - 1);
    }
}