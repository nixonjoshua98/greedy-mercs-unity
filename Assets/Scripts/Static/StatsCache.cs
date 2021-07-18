﻿using System;
using System.Linq;
using System.Numerics;
using System.Collections.Generic;

using UnityEngine;

using Random = UnityEngine.Random;

namespace GM
{
    using GM.Data;
    using GM.Artefacts;
    using GM.Units;

    public class StatsCache : MonoBehaviour
    {
        const float BASE_ENERGY_CAP = 50.0f;
        const float BASE_ENERGY_MIN = 1.0f;

        const float BASE_CRIT_CHANCE = 0.01f;
        const float BASE_CRIT_MULTIPLIER = 2.5f;

        const float BASE_BOSS_TIMER = 15.0f;

        static List<KeyValuePair<BonusType, double>> SkillBonus { get { return SkillsManager.Instance.Bonuses(); } }
        static List<KeyValuePair<BonusType, double>> ArmouryBonus { get { return UserData.Get().Armoury.Bonuses(); } }
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
            }

            public static double SkillDuration(SkillID skill)
            {
                SkillSO skillSo = StaticData.SkillList.Get(skill);

                return skillSo.Duration;
            }

            public static BigDouble AutoClickDamage()
            {
                return GetTapDamage() * SkillBonus(SkillID.AUTO_CLICK);
            }
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
        public static BigDouble BaseMercDamage(MercID merc)
        {
            MercState state = MercenaryManager.Instance.GetState(merc);
            MercData data = GameData.Get().Mercs.Get(merc);

            BigDouble baseDamage = data.BaseDamage > 0 ? data.BaseDamage : (data.UnlockCost / (10.0f + BigDouble.Log10(data.UnlockCost)));

            return Formulas.MercBaseDamage(baseDamage, state.Level);
        }

        public static BigDouble TotalMercDamage(MercID merc)
        {
            MercData data = GameData.Get().Mercs.Get(merc);

            BigDouble val = BaseMercDamage(merc);

            val *= MultiplyAllSources(BonusType.MERC_DAMAGE);
            val *= MultiplyAllSources(data.Attack.ToBonusType());

            return val;
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
    }
}