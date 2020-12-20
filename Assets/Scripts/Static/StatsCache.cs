using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

using UnityEngine;

public class StatsCache : MonoBehaviour
{
    static BonusesFromHeroes bonusesFromHeroes;

    static Stopwatch bonusesFromHeroesStopWatch;

    public static double GetHeroDamage(HeroID hero)
    {
        CalculateBonusesFromHeroes();

        return Formulas.CalcHeroDamage(hero) * bonusesFromHeroes.allSquadDamage;
    }

    public static double GetEnemyGold(int stage)
    {
        CalculateBonusesFromHeroes();

        return Formulas.CalcEnemyGold(stage) * bonusesFromHeroes.allGold;
    }

    public static double GetBossGold(int stage)
    {
        CalculateBonusesFromHeroes();

        return Formulas.CalcBossGold(stage) * bonusesFromHeroes.allGold;
    }

    public static void CalculateBonusesFromHeroes()
    {
        if (bonusesFromHeroesStopWatch == null || bonusesFromHeroesStopWatch.ElapsedMilliseconds >= 1000)
        {
            if (bonusesFromHeroesStopWatch == null)
                bonusesFromHeroesStopWatch = Stopwatch.StartNew();

            bonusesFromHeroesStopWatch.Restart();

            bonusesFromHeroes = ServerData.GetBonusesFromHeroes();
        }
    }
}
