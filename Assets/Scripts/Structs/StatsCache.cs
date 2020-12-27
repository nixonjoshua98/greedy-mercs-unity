using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class StatsCache : MonoBehaviour
{
    static Dictionary<BonusType, double> HeroBonus { get { return StaticData.GetBonusesFromHeroes(); } }
    static Dictionary<BonusType, double> RelicBonus { get { return GameState.Relics.CalculateBonuses(); } }


    static Dictionary<CharacterID, double> CharacterDamageCache = new Dictionary<CharacterID, double>();

    static double TotalCharacterDamage { get { return CharacterDamageCache.Values.Sum(); } }

    public static double GetHeroDamage(CharacterID chara)
    {
        CharacterDamageCache[chara] = Formulas.CalcHeroDamage(chara) * HeroBonus.GetValueOrDefault(BonusType.ALL_SQUAD_DAMAGE, 1) * RelicBonus.GetValueOrDefault(BonusType.ALL_SQUAD_DAMAGE, 1);

        return CharacterDamageCache[chara];
    }

    public static double GetEnemyGold(int stage)
    {
        return Formulas.CalcEnemyGold(stage) * HeroBonus.GetValueOrDefault(BonusType.ENEMY_GOLD, 1) * RelicBonus.GetValueOrDefault(BonusType.ENEMY_GOLD, 1);
    }

    public static double GetBossGold(int stage)
    {
        return Formulas.CalcBossGold(stage) * HeroBonus.GetValueOrDefault(BonusType.BOSS_GOLD, 1) * RelicBonus.GetValueOrDefault(BonusType.BOSS_GOLD, 1);
    }

    public static double GetTapDamage()
    {
        double dmg = Formulas.CalcTapDamage() * HeroBonus.GetValueOrDefault(BonusType.TAP_DAMAGE, 1) * RelicBonus.GetValueOrDefault(BonusType.TAP_DAMAGE, 1);

        // % bonus from heroes
        dmg += TotalCharacterDamage * HeroBonus.GetValueOrDefault(BonusType.HERO_TAP_DAMAGE_ADD, 0);

        return dmg;
    }
}
