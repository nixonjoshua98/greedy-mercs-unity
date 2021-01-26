using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace GreedyMercs
{
    public class StatsCache : MonoBehaviour
    {
        #region Cache
        static Dictionary<string, float> CacheTimers                            = new Dictionary<string, float>();
        static Dictionary<string, BigDouble> CachedDoubles                      = new Dictionary<string, BigDouble>();
        static Dictionary<string, Dictionary<BonusType, double>> CachedBonuses  = new Dictionary<string, Dictionary<BonusType, double>>();

        static Dictionary<BonusType, double> BonusFromCharacters
        {
            get
            {
                if (IsCacheOutdated("CHARACTERS", CachedBonuses))
                    CachedBonuses["CHARACTERS"] = GameState.Characters.CalcBonuses();

                return CachedBonuses["CHARACTERS"];
            }
        }

        static Dictionary<BonusType, double> BonusFromLoot
        {
            get
            {
                if (IsCacheOutdated("LOOT", CachedBonuses))
                    CachedBonuses["LOOT"] = GameState.Loot.CalcBonuses();

                return CachedBonuses["LOOT"];
            }
        }

        public static BigDouble ArmouryDamageMultiplier
        {
            get
            {
                if (IsCacheOutdated("ARMOURY_DAMAGE", CachedDoubles))
                    CachedDoubles["ARMOURY_DAMAGE"] = Math.Max(1.0f, GameState.Armoury.DamageBonus());

                return CachedDoubles["ARMOURY_DAMAGE"];
            }
        }
        #endregion

        static Dictionary<BonusType, double> BonusFromSkills { get { return GameState.Skills.CacBonuses(); } }

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

        public static void Clear()
        {
            CacheTimers.Clear();

            CachedDoubles.Clear();

            CachedBonuses.Clear();
        }

        public static class GoldUpgrades
        {
            #region Auto Tap Upgrade
            public static BigDouble AutoTapDamage() => AutoTaps() * StatsCache.GetTapDamage();
            public static double AutoTaps() => Formulas.GoldUpgrades.CalcAutoTaps() + AddictiveBonuses(BonusType.AUTO_TAPS);
            #endregion
        }

        public static class StageEnemy
        {
            #region Boss
            public static float BossTimer => 15.0f + (float)BonusFromLoot.GetOrVal(BonusType.BOSS_TIMER_DUR, 0);

            public static BigDouble GetEnemyGold(int stage) => Formulas.CalcEnemyGold(stage) * MultiplyBonuses(BonusType.ENEMY_GOLD, BonusType.ALL_GOLD);

            public static BigDouble GetBossGold(int stage) => Formulas.CalcBossGold(stage) * MultiplyBonuses(BonusType.BOSS_GOLD, BonusType.ALL_GOLD);
            #endregion
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
            SkillState state = GameState.Skills.Get(SkillID.GOLD_RUSH);

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