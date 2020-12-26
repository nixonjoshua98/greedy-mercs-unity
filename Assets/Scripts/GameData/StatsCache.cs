using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

using UnityEngine;

public class StatsCache : MonoBehaviour
{
    static Dictionary<BonusType, double> bonusesFromHeroes { get { return StaticData.GetBonusesFromHeroes(); } }

    public static double GetHeroDamage(CharacterID hero)
    {
        return Formulas.CalcHeroDamage(hero) * bonusesFromHeroes.GetValueOrDefault(BonusType.ALL_SQUAD_DAMAGE, 1);
    }

    // ===

    public static double GetEnemyGold(int stage)
    {
        return Formulas.CalcEnemyGold(stage) * bonusesFromHeroes.GetValueOrDefault(BonusType.ENEMY_GOLD, 1);
    }

    public static double GetBossGold(int stage)
    {
        return Formulas.CalcBossGold(stage) * bonusesFromHeroes.GetValueOrDefault(BonusType.BOSS_GOLD, 1);
    }

    // ===

    public static double GetTapDamage()
    {
        return Formulas.CalcTapDamage() * bonusesFromHeroes.GetValueOrDefault(BonusType.TAP_DAMAGE, 1);
    }
}
