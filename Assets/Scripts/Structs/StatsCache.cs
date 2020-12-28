using System.Linq;
using System.Collections.Generic;

using UnityEngine;

public class StatsCache : MonoBehaviour
{
    static Dictionary<BonusType, double> HeroBonus { get { return GameState.Characters.CalculateBonuses(); } }
    static Dictionary<BonusType, double> RelicBonus { get { return GameState.Relics.CalculateBonuses(); } }


    static Dictionary<CharacterID, BigDouble> CharacterDamageCache = new Dictionary<CharacterID, BigDouble>();

    static BigDouble TotalCharacterDamage { get { BigDouble total = 0; foreach (BigDouble dmg in CharacterDamageCache.Values) total += dmg; return total; } }

    public static BigDouble GetHeroDamage(CharacterID chara)
    {
        CharacterDamageCache[chara] = Formulas.CalcCharacterDamage(chara) * HeroBonus.GetValueOrDefault(BonusType.ALL_SQUAD_DAMAGE, 1) * RelicBonus.GetValueOrDefault(BonusType.ALL_SQUAD_DAMAGE, 1);

        return CharacterDamageCache[chara];
    }

    public static BigDouble GetEnemyGold(int stage)
    {
        return Formulas.CalcEnemyGold(stage) * HeroBonus.GetValueOrDefault(BonusType.ENEMY_GOLD, 1) * RelicBonus.GetValueOrDefault(BonusType.ENEMY_GOLD, 1);
    }

    public static BigDouble GetBossGold(int stage)
    {
        return Formulas.CalcBossGold(stage) * HeroBonus.GetValueOrDefault(BonusType.BOSS_GOLD, 1) * RelicBonus.GetValueOrDefault(BonusType.BOSS_GOLD, 1);
    }

    public static BigDouble GetTapDamage()
    {
        BigDouble dmg = Formulas.CalcTapDamage()  * HeroBonus.GetValueOrDefault(BonusType.TAP_DAMAGE, 1) * RelicBonus.GetValueOrDefault(BonusType.TAP_DAMAGE, 1);

        // % bonus from heroes
        dmg += TotalCharacterDamage * HeroBonus.GetValueOrDefault(BonusType.HERO_TAP_DAMAGE_ADD, 0);

        return dmg;
    }
}
