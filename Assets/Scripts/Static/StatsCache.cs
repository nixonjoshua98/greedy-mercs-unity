using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using UnityEngine;

using SkillData;
using CharacterData;

class CacheValue
{
    public float CachedAt;

    public BigDouble Value;

    public Dictionary<BonusType, double> Dict;
}

public class StatsCache : MonoBehaviour
{
    static Dictionary<string, CacheValue> CachedValues = new Dictionary<string, CacheValue>();

    static Dictionary<BonusType, double> BonusFromCharacters 
    { 
        get 
        {
            if (IsCacheOutdated("CHARACTERS"))
                CachedValues["CHARACTERS"].Dict = GameState.Characters.CalcBonuses();
            
            return CachedValues["CHARACTERS"].Dict;
        }
    }

    static Dictionary<BonusType, double> BonusFromLoot
    {
        get
        {
            if (IsCacheOutdated("LOOT"))
                CachedValues["LOOT"].Dict = GameState.Loot.CalcBonuses();

            return CachedValues["LOOT"].Dict;
        }
    }

    static Dictionary<BonusType, double> BonusFromSkills { get { return GameState.Skills.CacBonuses(); } }

    public static BigDouble TotalCharacterDamage { 
        get 
        { 
            BigDouble total = 0; 

            foreach (KeyValuePair<string, CacheValue> entry in CachedValues)
            {
                if (entry.Key.StartsWith("CHARACTER_DMG_"))
                {
                    total += entry.Value.Value;
                }
            }

            return total; 
        } 
    }

    public static void Clear()
    {
        CachedValues.Clear();
    }

    public static class GoldUpgrades
    {
        public static BigDouble AutoTapDamage() => AutoTaps() * StatsCache.GetTapDamage();
        public static double AutoTaps() => Formulas.GoldUpgrades.CalcAutoTaps() + AddictiveBonuses(BonusType.AUTO_TAPS);
    }

    // # === Energy === #
    public static double PlayerEnergyPerMinute() => 1.0f + AddictiveBonuses(BonusType.ENERGY_INCOME);
    public static double PlayerMaxEnergy()
    {
        int skillEnergy = GameState.Skills.Unlocked().Sum(item => item.EnergyGainedOnUnlock);

        return 50.0f + skillEnergy + AddictiveBonuses(BonusType.ENERGY_CAPACITY);
    }
    
    // === Skills === #
    public static double GoldRushBonus()
    {
        SkillState state = GameState.Skills.Get(SkillData.SkillID.GOLD_RUSH);

        return state.LevelData.BonusValue * MultiplyBonuses(BonusType.GOLD_RUSH_BONUS);
    }

    public static double SkillDuration(SkillID skill)
    {
        SkillSO state = StaticData.SkillList.Get(skill);

        return state.Duration + AddictiveBonuses(BonusType.GOLD_RUSH_DURATION);
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
            CharacterSO data = StaticData.CharacterList.Get(chara);

            CachedValues[key].Value = Formulas.CalcCharacterDamage(chara) * MultiplyBonuses(BonusType.MERC_DAMAGE, data.attackType) * GameState.Weapons.CalcBonuses(chara);
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
        {
            CachedValues["TAP_DAMAGE"].Value = (Formulas.CalcTapDamage() * MultiplyBonuses(BonusType.TAP_DAMAGE)) + GameState.Characters.CalcTapDamageBonus();
        }

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
            val *= BonusFromCharacters.GetOrVal(type, 1) * BonusFromLoot.GetOrVal(type, 1) * BonusFromSkills.GetOrVal(type, 1);

        return val;
    }

    public static double AddictiveBonuses(params BonusType[] types)
    {
        double val = 0.0f;

        foreach (BonusType type in types)
            val += BonusFromCharacters.GetOrVal(type, 0) + BonusFromLoot.GetOrVal(type, 0);

        return val;
    }
}
