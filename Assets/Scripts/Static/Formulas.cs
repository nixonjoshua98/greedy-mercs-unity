using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public static class Formulas
{
    // =====

    public static double CalcEnemyHealth(int stage) { return 15 * Mathf.Pow(1.2f, Mathf.Min(stage, 100)) * Mathf.Pow(1.15f, Mathf.Max(stage - 100, 0)); }

    // =====

    public static double CalcEnemyGold(int stage) { return CalcEnemyHealth(stage) * 0.008 + Mathf.Pow(1.1f, Mathf.Min(stage, 100)); }

    // ===

    public static double CalcHeroDamage(HeroID heroId) { return GameState.GetHeroState(heroId).level; }

    // ===
}
