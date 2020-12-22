using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

using UnityEngine;

public class StatsCache : MonoBehaviour
{
    static Dictionary<BonusType, double> bonusesFromHeroes { get { return ServerData.GetBonusesFromHeroes(); } }

    public static double GetHeroDamage(HeroID hero)
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
        return Formulas.CalcBossGold(stage);
    }

    // ===

    public static double GetTapDamage()
    {
        return Formulas.CalcTapDamage();
    }
}
