using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace GreedyMercs
{
    using GM.Armoury;
    using GM.Artefacts;

    public class StatsCache : MonoBehaviour
    {
        #region Cache
        static Dictionary<string, float> CacheTimers                            = new Dictionary<string, float>();
        static Dictionary<string, BigDouble> CachedDoubles                      = new Dictionary<string, BigDouble>();
        static Dictionary<string, Dictionary<BonusType, double>> CachedBonuses  = new Dictionary<string, Dictionary<BonusType, double>>();

        public static BigDouble ArmouryDamageMultiplier
        {
            get
            {
                if (IsCacheOutdated("ARMOURY_DAMAGE", CachedDoubles))
                    CachedDoubles["ARMOURY_DAMAGE"] = Math.Max(1.0f, ArmouryManager.Instance.DamageBonus());

                return CachedDoubles["ARMOURY_DAMAGE"];
            }
        }

        static Dictionary<BonusType, double> BonusFromLoot
        {
            get
            {
                if (IsCacheOutdated("LOOT", CachedBonuses))
                    CachedBonuses["LOOT"] = ArtefactManager.Instance.CalculateTotalBonuses();

                return CachedBonuses["LOOT"];
            }
        }
        static Dictionary<BonusType, double> BonusFromCharacters
        {
            get
            {
                if (IsCacheOutdated("CHARACTERS", CachedBonuses))
                    CachedBonuses["CHARACTERS"] = GameState.Characters.CalcBonuses();

                return CachedBonuses["CHARACTERS"];
            }
        }

        static Dictionary<BonusType, double> BonusFromSkills { get { return GameState.Skills.CalcBonuses(); } }

        public static BigDouble GetCritChance() => StaticData.BASE_CRIT_CHANCE + AddictiveBonuses(BonusType.CRIT_CHANCE);
        public static BigDouble GetCritDamage() => StaticData.BASE_CRIT_MULTIPLIER + AddictiveBonuses(BonusType.CRIT_DAMAGE);

        public static BigDouble TotalCharacterDPS
        {
            get
            {
                BigDouble total = 0;

                foreach (CharacterID chara in GameState.Characters.Unlocked())
                {
                    UpgradeState state = GameState.Characters.Get(chara);

                    if (CachedDoubles.TryGetValue(string.Format("CHARACTER_DMG_{0}", chara.ToString()), out BigDouble val))
                        total += val;

                    else
                        total += GetCharacterDamage(chara);
                }

                return total;
            }
        }
        #endregion

        public static void Clear()
        {
            CacheTimers.Clear();

            CachedDoubles.Clear();

            CachedBonuses.Clear();
        }

        public static class StageEnemy
        {
            public static float BossTimer => 15.0f + (float)BonusFromLoot.GetOrVal(BonusType.BOSS_TIMER_DURATION, 0);

            public static BigDouble GetEnemyGold(int stage) => Formulas.CalcEnemyGold(stage) * MultiplyBonuses(BonusType.ENEMY_GOLD, BonusType.ALL_GOLD);

            public static BigDouble GetBossGold(int stage) => Formulas.CalcBossGold(stage) * MultiplyBonuses(BonusType.BOSS_GOLD, BonusType.ALL_GOLD);
        }

        public static class Skills
        {
            public static double SkillBonus(SkillID skill)
            {
                switch (skill)
                {
                    case SkillID.GOLD_RUSH:
                        return GameState.Skills.Get(SkillID.GOLD_RUSH).LevelData.BonusValue * MultiplyBonuses(BonusType.GOLD_RUSH_BONUS);

                    case SkillID.AUTO_CLICK:
                        return GameState.Skills.Get(SkillID.AUTO_CLICK).LevelData.BonusValue * MultiplyBonuses(BonusType.AUTO_CLICK_BONUS);

                    default:
                        Debug.Break();

                        Debug.Log("Error: Skill not found");

                        return 0;
                }
            }

            public static double SkillDuration(SkillID skill)
            {
                switch (skill)
                {
                    case SkillID.GOLD_RUSH:
                        return StaticData.SkillList.Get(SkillID.GOLD_RUSH).Duration + AddictiveBonuses(BonusType.GOLD_RUSH_DURATION);

                    case SkillID.AUTO_CLICK:
                        return StaticData.SkillList.Get(SkillID.AUTO_CLICK).Duration + AddictiveBonuses(BonusType.AUTO_CLICK_DURATION);

                    default:
                        Debug.Break();

                        Debug.Log("Error: Skill not found");

                        return 0;
                }
            }

            public static BigDouble AutoClickDamage()
            {
                if (IsCacheOutdated("AUTO_CLICK_DMG", CachedDoubles))
                    CachedDoubles["AUTO_CLICK_DMG"] = GetTapDamage() * SkillBonus(SkillID.AUTO_CLICK);

                return CachedDoubles["AUTO_CLICK_DMG"];
            }
        }


        // # === Energy === #
        public static double PlayerEnergyPerMinute() => (1.0f + AddictiveBonuses(BonusType.ENERGY_INCOME));
        public static double PlayerMaxEnergy()
        {
            int skillEnergy = GameState.Skills.Unlocked().Sum(item => item.EnergyGainedOnUnlock);

            return 50.0f + skillEnergy + AddictiveBonuses(BonusType.ENERGY_CAPACITY);
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

        public static BigDouble GetCharacterDamage(CharacterID chara)
        {
            string key = "CHARACTER_DMG_" + chara.ToString();

            if (IsCacheOutdated(key, CachedDoubles))
            {
                CharacterSO data = StaticData.CharacterList.Get(chara);

                CachedDoubles[key] = Formulas.CalcCharacterDamage(chara) * MultiplyBonuses(BonusType.MERC_DAMAGE, data.attackType) * ArmouryDamageMultiplier;
            }

            return CachedDoubles[key];
        }

        public static BigDouble GetTapDamage()
        {
            string key = "TAP_DAMAGE";

            if (IsCacheOutdated(key, CachedDoubles))
                CachedDoubles[key] = (Formulas.CalcTapDamage() * MultiplyBonuses(BonusType.TAP_DAMAGE)) + GameState.Characters.CalcTapDamageBonus();

            return CachedDoubles[key];
        }

        static bool IsCacheOutdated<TVal>(string key, Dictionary<string, TVal> valueCache)
        {
            if (!valueCache.ContainsKey(key) || !CacheTimers.ContainsKey(key) || (Time.realtimeSinceStartup - CacheTimers[key]) >= 0.5f)
            {
                CacheTimers[key] = Time.realtimeSinceStartup;

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
}