using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace GM
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

        const float BASE_BOSS_TIMER = 15.0f;

        static List<KeyValuePair<BonusType, double>> SkillBonus { get { return SkillsManager.Instance.Bonuses(); } }
        static List<KeyValuePair<BonusType, double>> ArmouryBonus { get { return ArmouryManager.Instance.Bonuses(); } }
        static List<KeyValuePair<BonusType, double>> ArtefactBonus { get { return ArtefactManager.Instance.Bonuses(); } }
        static List<KeyValuePair<BonusType, double>> CharacterBonus { get { return MercenaryManager.Instance.Bonuses(); } }

        public static BigDouble ArmouryMercDamageMultiplier { get { return AddSource(BonusType.MERC_DAMAGE, ArmouryBonus); } }

        public static class StageEnemy
        {
            public static BigDouble GetEnemyGold(int stage)
            {
                return (
                    Formulas.CalcEnemyGold(stage) *

                    MultiplyAllSources(BonusType.ENEMY_GOLD) * 
                    MultiplyAllSources(BonusType.ALL_GOLD)
                    );
            }

            public static BigDouble GetBossGold(int stage)
            {
                return (
                    Formulas.CalcBossGold(stage) *

                    MultiplyAllSources(BonusType.BOSS_GOLD) *
                    MultiplyAllSources(BonusType.ALL_GOLD)
                    );
            }
        }

        public static class Skills
        {
            public static double SkillBonus(SkillID skill)
            {
                return SkillsManager.Instance.Get(skill).LevelData.BonusValue;

                switch (skill)
                {
                    case SkillID.GOLD_RUSH:
                        return SkillsManager.Instance.Get(SkillID.GOLD_RUSH).LevelData.BonusValue * MultiplyAllSources(BonusType.GOLD_RUSH_BONUS);

                    case SkillID.AUTO_CLICK:
                        return SkillsManager.Instance.Get(SkillID.AUTO_CLICK).LevelData.BonusValue * MultiplyAllSources(BonusType.AUTO_CLICK_BONUS);

                    default:
                        Debug.Break();

                        Debug.Log("Error: Skill not found");

                        return 0;
                }
            }

            public static double SkillDuration(SkillID skill)
            {
                SkillSO skillSo = StaticData.SkillList.Get(skill);

                return skillSo.Duration;

                switch (skill)
                {
                    case SkillID.GOLD_RUSH:
                        return skillSo.Duration + AddAllSources(BonusType.GOLD_RUSH_DURATION);

                    case SkillID.AUTO_CLICK:
                        return skillSo.Duration + AddAllSources(BonusType.AUTO_CLICK_DURATION);
                }

                return 0;
            }

            public static BigDouble AutoClickDamage()
            {
                return GetTapDamage() * SkillBonus(SkillID.AUTO_CLICK);
            }
        }

        // = = = Enemies = = = //
        public static float BossTimer()
        {
            return BASE_BOSS_TIMER + (float) AddAllSources(BonusType.BOSS_TIMER_DURATION);
        }


        // # === Energy === #
        public static double EnergyPerMinute()
        {
            double flatExtraCapacity = AddAllSources(BonusType.FLAT_ENERGY_INCOME);
            double percentExtraCapacity = MultiplyAllSources(BonusType.PERCENT_ENERGY_INCOME);

            return (BASE_ENERGY_MIN + flatExtraCapacity) * percentExtraCapacity;
        }

        public static double MaxEnergyCapacity()
        {
            int energyFromSkills = SkillsManager.Instance.Unlocked().Sum(item => item.EnergyGainedOnUnlock);

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
            MercData data = StaticData.Mercs.GetMerc(chara);

            BigDouble val = BaseMercDamage(chara);

            val *= MultiplyAllSources(BonusType.MERC_DAMAGE);
            val *= MultiplyAllSources(data.AttackType);

            return val;
        }

        public static BigDouble TotalMercDamage()
        {
            BigDouble total = 0;

            foreach (CharacterID chara in MercenaryManager.Instance.Unlocked)
                total += TotalMercDamage(chara);

            return total;
        }

        public static BigInteger GetPrestigePoints(int stage)
        {
            BigDouble big = Formulas.CalcPrestigePoints(stage).ToBigDouble() * MultiplyAllSources(BonusType.CASH_OUT_BONUS);

            return BigInteger.Parse(big.Ceiling().ToString("F0"));
        }

        public static BigDouble GetTapDamage()
        {
            return (Formulas.CalcTapDamage() * MultiplyAllSources(BonusType.TAP_DAMAGE)) + GameState.Characters.CalcTapDamageBonus();
        }


        // === Helper Methods ===

        static double MultiplyAllSources(BonusType type)
        {
            return (
                MultiplySource(type, SkillBonus) *
                MultiplySource(type, ArmouryBonus) *
                MultiplySource(type, ArtefactBonus) *
                MultiplySource(type, CharacterBonus)
                );
        }

        static double AddAllSources(BonusType type)
        {
            return (
                AddSource(type, SkillBonus) +
                AddSource(type, ArmouryBonus) +
                AddSource(type, ArtefactBonus) +
                AddSource(type, CharacterBonus)
                );
        }

        static double AddSource(BonusType type, List<KeyValuePair<BonusType, double>> ls)
        {
            double val = 0;

            foreach (KeyValuePair<BonusType, double> pair in ls)
            {
                if (pair.Key == type)
                {
                    val += pair.Value;
                }
            }

            return val;
        }

        static double MultiplySource(BonusType type, List<KeyValuePair<BonusType, double>> ls)
        {
            double val = 1;

            foreach (KeyValuePair<BonusType, double> pair in ls)
            {
                if (pair.Key == type)
                {
                    val *= pair.Value;
                }
            }

            return val;
        }

        static double MultiplySource(BonusType type, List<KeyValuePair<BonusType, double>> ls, string name)
        {
            double val = 1;

            Debug.Log(name);

            foreach (KeyValuePair<BonusType, double> pair in ls)
            {
                Debug.Log(pair.Key + " " + pair.Value);

                if (pair.Key == type)
                {
                    val *= pair.Value;
                }
            }

            return val;
        }
    }
}