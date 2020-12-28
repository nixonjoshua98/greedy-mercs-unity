using System.Collections.Generic;

using UnityEngine;

class CacheValue
{
    public float CachedAt;

    public BigDouble Value;
}


public class StatsCache : MonoBehaviour
{
    // === Cache Dictionaries ===
    static Dictionary<string, CacheValue>       StringCache             = new Dictionary<string, CacheValue>();
    static Dictionary<CharacterID, CacheValue>  CharacterDamageCache    = new Dictionary<CharacterID, CacheValue>();

    static float lastCharBonusCalc  = 0;
    static float lastRelicBonucCalc = 0;

    static Dictionary<BonusType, double> cachedHeroBonus    = new Dictionary<BonusType, double>();
    static Dictionary<BonusType, double> cachedRelicBonus   = new Dictionary<BonusType, double>();

    static Dictionary<BonusType, double> HeroBonus { 
        get {
            if ((Time.realtimeSinceStartup - lastCharBonusCalc) >= 1.0f)
            {
                lastCharBonusCalc = Time.realtimeSinceStartup;

                cachedHeroBonus = GameState.Characters.CalculateBonuses();
            }

            return cachedHeroBonus;
        } 
    }

    static Dictionary<BonusType, double> RelicBonus
    {
        get
        {
            if ((Time.realtimeSinceStartup - lastRelicBonucCalc) >= 1.0f)
            {
                lastRelicBonucCalc = Time.realtimeSinceStartup;

                cachedRelicBonus = GameState.Relics.CalculateBonuses();
            }

            return cachedRelicBonus;
        }
    }
    static BigDouble TotalCharacterDamage { get { BigDouble total = 0; foreach (CacheValue entry in CharacterDamageCache.Values) total += entry.Value; return total; } }

    public static void ClearCache()
    {
        lastRelicBonucCalc = lastCharBonusCalc = 0;

        StringCache = new Dictionary<string, CacheValue>();

        CharacterDamageCache = new Dictionary<CharacterID, CacheValue>();
    }

    public static BigDouble GetHeroDamage(CharacterID chara)
    {
        if (IsCacheOutdated(chara, CharacterDamageCache))
        {
            var data = CharacterResources.GetCharacter(chara);

            BigDouble val = Formulas.CalcCharacterDamage(chara) 

                * HeroBonus.GetValueOrDefault(BonusType.ALL_MERC_DAMAGE, 1) 
                * HeroBonus.GetValueOrDefault(data.AttackType, 1)

                * RelicBonus.GetValueOrDefault(BonusType.ALL_MERC_DAMAGE, 1)
                * RelicBonus.GetValueOrDefault(data.AttackType, 1);

            CharacterDamageCache[chara].Value = val;
        }

        return CharacterDamageCache[chara].Value;
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
        if (IsCacheOutdated("TAP_DAMAGE", StringCache))
        {
            BigDouble val = Formulas.CalcTapDamage() * HeroBonus.GetValueOrDefault(BonusType.TAP_DAMAGE, 1) * RelicBonus.GetValueOrDefault(BonusType.TAP_DAMAGE, 1);

            val += TotalCharacterDamage * HeroBonus.GetValueOrDefault(BonusType.HERO_TAP_DAMAGE_ADD, 0);

            StringCache["TAP_DAMAGE"].Value = val;
        }

        return StringCache["TAP_DAMAGE"].Value;
    }

    static bool IsCacheOutdated<T>(T key, Dictionary<T, CacheValue> dict)
    {
        if (!dict.ContainsKey(key) || (Time.realtimeSinceStartup - dict[key].CachedAt) >= 1.0f)
        {
            dict[key] = new CacheValue { CachedAt = Time.realtimeSinceStartup };

            return true;
        }

        return false;
    }
}
