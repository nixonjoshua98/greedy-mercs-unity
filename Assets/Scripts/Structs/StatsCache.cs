﻿using System.Numerics;

using System.Collections.Generic;

using UnityEngine;

using CharacterID = CharacterData.CharacterID;

class CacheValue
{
    public float CachedAt;

    public BigDouble Value;

    public Dictionary<BonusType, double> Dict;
}

public class StatsCache : MonoBehaviour
{
    static Dictionary<string, CacheValue> CachedValues = new Dictionary<string, CacheValue>();

    static Dictionary<BonusType, double> CharacterBonus 
    { 
        get 
        {
            if (IsCacheOutdated("CHARACTERS"))
                CachedValues["CHARACTERS"].Dict = GameState.Characters.CalculateBonuses();
            
            return CachedValues["CHARACTERS"].Dict;
        }
    }

    static Dictionary<BonusType, double> RelicBonus
    {
        get
        {
            if (IsCacheOutdated("RELICS"))
                CachedValues["RELICS"].Dict = GameState.Relics.CalculateBonuses();

            return CachedValues["RELICS"].Dict;
        }
    }

    static BigDouble TotalCharacterDamage { 
        get 
        { 
            BigDouble total = 0; 
            
            foreach (CacheValue entry in CachedValues.Values)
                total += entry.Value; 

            return total; 
        } 
    }

    public static void Clear()
    {
        CachedValues.Clear();
    }

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

    public static BigInteger GetPrestigePoints(int stage)
    {
        BigDouble big = Formulas.CalcPrestigePoints(stage).ToBigDouble() * MultiplyBonuses(BonusType.CASH_OUT_BONUS);

        return BigInteger.Parse(big.Ceiling().ToString("F0"));
    }

    public static BigDouble GetCritChance()
    {
        return StaticData.BASE_CRIT_CHANCE + AddictiveBonuses(BonusType.CRIT_CHANCE);
    }

    public static BigDouble GetCritDamage()
    {
        return StaticData.BASE_CRIT_MULTIPLIER + AddictiveBonuses(BonusType.CRIT_DAMAGE);
    }

    public static BigDouble GetCharacterDamage(CharacterID chara)
    {
        string key = "CHARACTER_DMG_" + chara.ToString();

        if (IsCacheOutdated(key))
        {
            ScriptableCharacter data = CharacterResources.Instance.GetCharacter(chara);

            CachedValues[key].Value = Formulas.CalcCharacterDamage(chara) * MultiplyBonuses(chara, BonusType.MERC_DAMAGE, data.attackType);
        }

        return CachedValues[key].Value;
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
        if (IsCacheOutdated("TAP_DAMAGE"))
            CachedValues["TAP_DAMAGE"].Value = Formulas.CalcTapDamage() * MultiplyBonuses(BonusType.TAP_DAMAGE) + (TotalCharacterDamage * AddictiveBonuses(BonusType.HERO_TAP_DAMAGE_ADD));

        return CachedValues["TAP_DAMAGE"].Value;
    }

    // === Internal Methods ===
    static bool IsCacheOutdated(string key)
    {
        if (!CachedValues.ContainsKey(key) || (Time.realtimeSinceStartup - CachedValues[key].CachedAt) >= 0.5f)
        {
            CachedValues[key] = new CacheValue { CachedAt = Time.realtimeSinceStartup };

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

    public static double MultiplyBonuses(CharacterID character, params BonusType[] types)
    {
        return MultiplyBonuses(types) * GameState.Weapons.CalculateDamageBonus(character);
    }

    public static double AddictiveBonuses(params BonusType[] types)
    {
        double val = 0.0f;

        foreach (BonusType type in types)
            val += CharacterBonus.GetValueOrDefault(type, 0) + RelicBonus.GetValueOrDefault(type, 0);

        return val;
    }
}
