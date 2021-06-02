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
    using GM.Characters;

    public class StatsCache : MonoBehaviour
    {
        const float BASE_ENERGY_CAP = 50.0f;
        const float BASE_ENERGY_MIN = 1.0f;

        const float BASE_CRIT_CHANCE = 0.01f;
        const float BASE_CRIT_MULTIPLIER = 2.5f;

        static Dictionary<string, float> CacheTimers                            = new Dictionary<string, float>();
        static Dictionary<string, BigDouble> CachedDoubles                      = new Dictionary<string, BigDouble>();
        static Dictionary<string, Dictionary<BonusType, double>> CachedBonuses  = new Dictionary<string, Dictionary<BonusType, double>>();

        public static BigDouble ArmouryDamageMultiplier
        {
            get
            {
                return Math.Max(1.0f, ArmouryManager.Instance.DamageBonus());
            }
        }

        static Dictionary<BonusType, double> BonusFromArmoury { get { return ArmouryManager.Instance.CalculateBonuses(); } }
        static Dictionary<BonusType, double> BonusFromArtefacts { get { return ArtefactManager.Instance.CalculateBonuses(); } }
        static Dictionary<BonusType, double> BonusFromCharacterPassives { get { return MercenaryManager.Instance.CalculateBonuses(); } }
        static Dictionary<BonusType, double> BonusFromSkills { get { return GameState.Skills.CalculateBonuses(); } }

        public static BigDouble TotalCharacterDPS
        {
            get
            {
                BigDouble total = 0;

                foreach (CharacterID chara in MercenaryManager.Instance.Unlocked)
                {                   
                    total += TotalMercDamage(chara);
                }

                return total;
            }
        }

        public static void Clear()
        {
            CacheTimers.Clear();

            CachedDoubles.Clear();

            CachedBonuses.Clear();
        }

        public static class StageEnemy
        {
            public static float BossTimer => 15.0f + (float)BonusFromArtefacts.Get(BonusType.BOSS_TIMER_DURATION, 0);

            public static BigDouble GetEnemyGold(int stage) => Formulas.CalcEnemyGold(stage) * AddAllSources(BonusType.ENEMY_GOLD) * AddAllSources(BonusType.ALL_GOLD);

            public static BigDouble GetBossGold(int stage) => Formulas.CalcBossGold(stage) * AddAllSources(BonusType.BOSS_GOLD) * AddAllSources(BonusType.ALL_GOLD);
        }

        public static class Skills
        {
            public static double SkillBonus(SkillID skill)
            {
                switch (skill)
                {
                    case SkillID.GOLD_RUSH:
                        return GameState.Skills.Get(SkillID.GOLD_RUSH).LevelData.BonusValue * MultiplyAllSources(BonusType.GOLD_RUSH_BONUS);

                    case SkillID.AUTO_CLICK:
                        return GameState.Skills.Get(SkillID.AUTO_CLICK).LevelData.BonusValue * MultiplyAllSources(BonusType.AUTO_CLICK_BONUS);

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
                        return StaticData.SkillList.Get(SkillID.GOLD_RUSH).Duration + AddAllSources(BonusType.GOLD_RUSH_DURATION);

                    case SkillID.AUTO_CLICK:
                        return StaticData.SkillList.Get(SkillID.AUTO_CLICK).Duration + AddAllSources(BonusType.AUTO_CLICK_DURATION);

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
        public static double EnergyPerMinute()
        {
            double flatExtraCapacity = AddAllSources(BonusType.FLAT_ENERGY_INCOME, 0);
            double percentExtraCapacity = MultiplyAllSources(BonusType.PERCENT_ENERGY_INCOME, 1);

            return (BASE_ENERGY_MIN + flatExtraCapacity) * percentExtraCapacity;
        }

        public static double MaxEnergyCapacity()
        {
            int energyFromSkills = GameState.Skills.Unlocked().Sum(item => item.EnergyGainedOnUnlock);

            double flatExtraCapacity = AddAllSources(BonusType.FLAT_ENERGY_CAPACITY);
            double percentExtraCapacity = MultiplyAllSources(BonusType.PERCENT_ENERGY_CAPACITY);

            return (BASE_ENERGY_CAP + energyFromSkills + flatExtraCapacity) * percentExtraCapacity;
        }

        // = = = Critical Hits = = = //
        public static BigDouble CriticalHitChance()
        {
            return BASE_CRIT_CHANCE + AddAllSources(BonusType.FLAT_CRIT_CHANCE);
        }

        public static BigDouble CriticalHitMultiplier()
        {
            return BASE_CRIT_MULTIPLIER + MultiplyAllSources(BonusType.FLAT_CRIT_DMG_MULT);
        }


        public static bool ApplyCritHit(ref BigDouble val)
        {
            BigDouble critChance = CriticalHitChance();

            if (critChance >= 1.0 || Random.Range(0.0f, 1.0f) < critChance)
            {
                val *= CriticalHitMultiplier();

                return true;
            }

            return false;
        }

        // = = = Mercs = = = //
        public static BigDouble BaseMercDamage(CharacterID chara)
        {
            MercState state = MercenaryManager.Instance.GetState(chara);
            MercData data   = StaticData.Mercs.GetMerc(chara);

            BigDouble baseDamage = data.BaseDamage > 0 ? data.BaseDamage : (data.UnlockCost / (10.0f + BigDouble.Log10(data.UnlockCost)));

            return Formulas.MercBaseDamage(baseDamage, state.Level);
        }

        public static BigDouble TotalMercDamage(CharacterID chara)
        {
            MercState state = MercenaryManager.Instance.GetState(chara);

            BigDouble val = BaseMercDamage(chara);

            val *= MultiplyAllSources(BonusType.MERC_DAMAGE);
            val *= MultiplyAllSources(state.svrData.AttackType);

            return val;
        }

        public static BigInteger GetPrestigePoints(int stage)
        {
            BigDouble big = Formulas.CalcPrestigePoints(stage).ToBigDouble() * MultiplyAllSources(BonusType.CASH_OUT_BONUS);

            return BigInteger.Parse(big.Ceiling().ToString("F0"));
        }

        public static BigDouble GetTapDamage()
        {
            string key = "TAP_DAMAGE";

            if (IsCacheOutdated(key, CachedDoubles))
                CachedDoubles[key] = (Formulas.CalcTapDamage() * MultiplyAllSources(BonusType.TAP_DAMAGE)) + GameState.Characters.CalcTapDamageBonus();

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

        static double MultiplyAllSources(BonusType type)
        {
            return (
                BonusFromCharacterPassives.Get(type, 1) * 
                BonusFromArtefacts.Get(type, 1) *
                BonusFromSkills.Get(type, 1) *
                BonusFromArmoury.Get(type, 1)
                );
        }

        static double MultiplyAllSources(BonusType type, double defaultGetValue = 1)
        {
            return (
                BonusFromSkills.Get(type, defaultGetValue) *
                BonusFromArmoury.Get(type, defaultGetValue) *
                BonusFromArtefacts.Get(type, defaultGetValue) *
                BonusFromCharacterPassives.Get(type, defaultGetValue)
                );
        }

        static double AddAllSources(BonusType type, double defaultGetValue = 0)
        {
            return (
                BonusFromSkills.Get(type, defaultGetValue) +
                BonusFromArmoury.Get(type, defaultGetValue) +
                BonusFromArtefacts.Get(type, defaultGetValue) +
                BonusFromCharacterPassives.Get(type, defaultGetValue)
                );
        }
    }
}