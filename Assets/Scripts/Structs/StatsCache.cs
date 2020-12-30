using System.Collections.Generic;

using UnityEngine;

class CachedValue
{
    public float CachedAt;

    public Dictionary<BonusType, double> CachedDict;
}

public class StatsCache : MonoBehaviour
{
    static Dictionary<CharacterID, BigDouble>   CharacterDamageCache    = new Dictionary<CharacterID, BigDouble>();
    static Dictionary<string, CachedValue>      CachedBonuses           = new Dictionary<string, CachedValue>();

    static Dictionary<BonusType, double> CharacterBonus 
    { 
        get 
        {
            if (IsCacheOutdated("CHARACTERS"))
                CachedBonuses["CHARACTERS"].CachedDict = GameState.Characters.CalculateBonuses();

            return CachedBonuses["CHARACTERS"].CachedDict;
        }
    }

    static Dictionary<BonusType, double> RelicBonus
    {
        get
        {
            if (IsCacheOutdated("RELICS"))
                CachedBonuses["RELICS"].CachedDict = GameState.Relics.CalculateBonuses();

            return CachedBonuses["RELICS"].CachedDict;
        }
    }

    public static void Clear()
    {
        CachedBonuses.Clear();
        CharacterDamageCache.Clear();
    }

    static BigDouble TotalCharacterDamage { get { BigDouble total = 0; foreach (BigDouble entry in CharacterDamageCache.Values) total += entry; return total; } }

    public static bool ApplyCritHit(ref BigDouble val)
    {
        BigDouble critChance = GetCritChance();

        if (critChance >= 1.0 || Random.Range(0.0f, 1.0f) < critChance)
        {
            val *= GetCritDamage();

            return true;
        }

        return false;
    }

    // === Calculations ===

    public static BigDouble GetCritChance()
    {
        return StaticData.BASE_CRIT_CHANCE + AddictiveBonuses(BonusType.CRIT_CHANCE);
    }

    public static BigDouble GetCritDamage()
    {
        return StaticData.BASE_CRIT_MULTIPLIER + AddictiveBonuses(BonusType.CRIT_DAMAGE);
    }

    public static BigDouble GetHeroDamage(CharacterID chara)
    {
        CharacterStaticData staticData = CharacterResources.GetCharacter(chara);

        CharacterDamageCache[chara] = Formulas.CalcCharacterDamage(chara) * MultiplyBonuses(BonusType.ALL_MERC_DAMAGE, staticData.AttackType);

        return CharacterDamageCache[chara];
    }

    public static BigDouble GetEnemyGold(int stage)
    {
        return Formulas.CalcEnemyGold(stage) * MultiplyBonuses(BonusType.ENEMY_GOLD, BonusType.ALL_GOLD);
    }

    public static BigDouble GetBossGold(int stage)
    {
        return Formulas.CalcBossGold(stage) * MultiplyBonuses(BonusType.BOSS_GOLD, BonusType.ALL_GOLD);
    }

    public static BigDouble GetTapDamage()
    {
        return Formulas.CalcTapDamage() * MultiplyBonuses(BonusType.TAP_DAMAGE) + (TotalCharacterDamage * AddictiveBonuses(BonusType.HERO_TAP_DAMAGE_ADD));
    }

    // === Internal Methods ===
    static bool IsCacheOutdated(string key)
    {
        if (!CachedBonuses.ContainsKey(key) || (Time.realtimeSinceStartup - CachedBonuses[key].CachedAt) >= 1.0f)
        {
            CachedBonuses[key] = new CachedValue { CachedAt = Time.realtimeSinceStartup };

            return true;
        }

        return false;
    }


    // === Helper Methods ===
    public static double MultiplyBonuses(params BonusType[] types)
    {
        double val = 1.0f;

        foreach (BonusType type in types)
            val *= CharacterBonus.GetValueOrDefault(type, 1) * RelicBonus.GetValueOrDefault(type, 1);

        return val;
    }

    public static double AddictiveBonuses(params BonusType[] types)
    {
        double val = 0.0f;

        foreach (BonusType type in types)
            val += CharacterBonus.GetValueOrDefault(type, 0) + RelicBonus.GetValueOrDefault(type, 0);

        return val;
    }
}
