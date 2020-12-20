using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

using UnityEngine;

public class StatsCache : MonoBehaviour
{
    static Dictionary<BonusType, double> _bonusesFromHeroes; // Should not be accessed directly

    static Dictionary<BonusType, double> bonusesFromHeroes
    {
        get
        {
            if (bonusesFromHeroesStopWatch == null || bonusesFromHeroesStopWatch.ElapsedMilliseconds >= 1000)
            {
                if (bonusesFromHeroesStopWatch == null)
                    bonusesFromHeroesStopWatch = Stopwatch.StartNew();

                bonusesFromHeroesStopWatch.Restart();

                _bonusesFromHeroes = ServerData.GetBonusesFromHeroes();
            }

            return _bonusesFromHeroes;
        }
    }

    static Stopwatch bonusesFromHeroesStopWatch;

    public static double GetHeroDamage(HeroID hero)
    {
        return Formulas.CalcHeroDamage(hero) * bonusesFromHeroes.GetValueOrDefault(BonusType.ALL_SQUAD_DAMAGE, 1);
    }

    public static double GetEnemyGold(int stage)
    {
        return Formulas.CalcEnemyGold(stage) * bonusesFromHeroes.GetValueOrDefault(BonusType.ENEMY_GOLD, 1);
    }

    public static double GetBossGold(int stage)
    {
        return Formulas.CalcBossGold(stage);
    }
}
