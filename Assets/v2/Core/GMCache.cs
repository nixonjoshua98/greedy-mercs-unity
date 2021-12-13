using GM.Common;
using GM.Common.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GM.Armoury.Data;

namespace GM.Core
{
    public class GMCache : GMClass
    {
        // Temp
        public bool ApplyCritHit(ref BigDouble val)
        {
            float critChance = CriticalHitChance;

            if (MathUtils.PercentChance(critChance))
            {
                val *= CriticalHitMultiplier;

                return true;
            }

            return false;
        }

        GM.Common.TTLCache cache = new GM.Common.TTLCache();

        IEnumerable<KeyValuePair<BonusType, BigDouble>> UpgradeBonuses => App.Data.Upgrades.Upgrades.Values.Where(x => x.Level > 0).Select(x => new KeyValuePair<BonusType, BigDouble>(x.BonusType, x.Value));
        List<KeyValuePair<BonusType, BigDouble>> MercPassiveBonuses
        {
            get
            {
                List<KeyValuePair<BonusType, BigDouble>> ls = new List<KeyValuePair<BonusType, BigDouble>>();

                App.Data.Mercs.UnlockedMercs.ForEach(m => ls.AddRange(m.UnlockedPassives.Select(x => new KeyValuePair<BonusType, BigDouble>(x.Type, x.Value))));

                return ls;
            }
        }
        IEnumerable<KeyValuePair<BonusType, BigDouble>> ArtefactBonuses => App.Data.Artefacts.UserOwnedArtefacts.Select(s => new KeyValuePair<BonusType, BigDouble>(s.Bonus, s.Effect));
        IEnumerable<KeyValuePair<BonusType, BigDouble>> ArmouryBonuses => App.Data.Armoury.UserItems.Select(x => new KeyValuePair<BonusType, BigDouble>(x.BonusType, x.BonusValue));

        Dictionary<BonusType, BigDouble> CombinedBonuses
        {
            get
            {
                var ls = MercPassiveBonuses
                    .Concat(ArtefactBonuses)
                    .Concat(UpgradeBonuses)
                    .Concat(ArmouryBonuses);

                return CreateBonusDictionary(ls);
            }
        }

        public BigDouble EnemyHealthAtStage(int stage)
        {
            return GameFormulas.EnemyHealth(stage);
        }

        public BigDouble StageBossHealthAtStage(int stage)
        {
            return GameFormulas.BossHealth(stage);
        }

        public BigDouble GoldPerEnemyAtStage(int stage)
        {
            return GameFormulas.CalcEnemyGold(stage) * CombinedBonuses.Get(BonusType.MULTIPLY_ENEMY_GOLD, 1) * CombinedBonuses.Get(BonusType.MULTIPLY_ALL_GOLD, 1);
        }

        public BigDouble GoldPerStageBossAtStage(int stage)
        {
            return GameFormulas.CalcBossGold(stage) * CombinedBonuses.Get(BonusType.MULTIPLY_BOSS_GOLD, 1) * CombinedBonuses.Get(BonusType.MULTIPLY_ALL_GOLD, 1);
        }

        public BigInteger PrestigePointsAtStage(int stage)
        {
            BigDouble big = GameFormulas.CalcPrestigePoints(stage).ToBigDouble() * CombinedBonuses.Get(BonusType.MULTIPLY_PRESTIGE_BONUS, 1);

            return big.FloorToBigInteger();
        }

        #region Critical Hit
        public float CriticalHitChance
        {
            get
            {
                return (float)(Constants.BASE_CRIT_CHANCE + CombinedBonuses.Get(BonusType.FLAT_CRIT_CHANCE, 0)).ToDouble();
            }
        }

        public BigDouble CriticalHitMultiplier
        {
            get
            {
                return Constants.BASE_CRIT_MULTIPLIER + CombinedBonuses.Get(BonusType.MULTIPLY_CRIT_DMG, 1);
            }
        }
        #endregion

        #region Minor Tap Upgrade
        public BigDouble MinorTapUpgradeCost(int levels) => GameFormulas.MinorTapUpgradeCost(App.Data.Upgrades.MinorTapUpgrade.Level, levels);
        public BigDouble MinorTapUpgradeDamage => GameFormulas.MinorTapUpgradeBonusValue(App.Data.Upgrades.MinorTapUpgrade.Level);
        #endregion

        #region Major Tap Upgrade
        public BigDouble MajorTapUpgradeCost(int levels) => GameFormulas.MajorTapUpgradeCost(App.Data.Upgrades.MajorTapUpgrade.Level, levels);
        public BigDouble MajorTapUpgradeDamage => GameFormulas.MajorTapUpgradeBonusValue(App.Data.Upgrades.MajorTapUpgrade.Level);
        #endregion

        #region Artefacts
        public BigDouble ArtefactBaseEffect(Artefacts.Data.ArtefactData art)
        {
            return cache.Get<BigDouble>($"ArtefactBaseEffect/{art.CurrentLevel}/{art.BaseEffect}/{art.LevelEffect}", 60, () => GameFormulas.ArtefactBonusValue(art.CurrentLevel, art.BaseEffect, art.LevelEffect));
        }

        public BigDouble ArtefactEffect(Artefacts.Data.ArtefactData art)
        {
            return ArtefactBaseEffect(art);
        }

        public BigInteger ArtefactUnlockCost(int owned) => GameFormulas.ArtefactUnlockCost(owned);

        public BigInteger ArtefactUpgradeCost(GM.Artefacts.Data.ArtefactData data, int levels)
        {
            return GameFormulas.ArtefactUpgradeCost(data.CurrentLevel, levels, data.CostExpo, data.CostCoeff);
        }
        #endregion

        #region Armoury
        public double ArmouryItemBonusValue(ArmouryItemData item)
        {
            return cache.Get<double>($"ArmouryItemBonusValue/{item.CurrentLevel}/{item.BaseEffect}/{item.LevelEffect}", lifetime: 60,
                () => GameFormulas.ArmouryItemBonusValue(item.CurrentLevel, item.NumOwned, item.BaseEffect, item.LevelEffect));
        }

        public int ArmouryItemUpgradeCost(ArmouryItemData item)
        {
            return cache.Get<int>($"ArmouryItemUpgradeCost/{item.CurrentLevel}", lifetime: 60,
                () => GameFormulas.ArmouryItemUpgradeCost(item.CurrentLevel));
        }
        #endregion

        #region Mercs
        /// <summary>Upgrade cost for merc. CurrentLevel -> (CurrentLevel + levels)</summary>
        public BigDouble MercUpgradeCost(GM.Mercs.Data.MercData merc, int levels)
        {
            return GameFormulas.MercUpgradeCost(merc.CurrentLevel, levels, merc.UnlockCost);
        }

        /// <summary>Base merc damage for current level. Does not apply any bonuses</summary>
        public BigDouble MercBaseDamage(GM.Mercs.Data.MercData merc)
        {
            return cache.Get<BigDouble>($"MercBaseDamage/{merc.Id}/{merc.CurrentLevel}", lifetime: 60, () => GameFormulas.MercBaseDamageAtLevel(merc.BaseDamage, merc.CurrentLevel));
        }

        public BigDouble MercDamage(GM.Mercs.Data.MercData merc)
        {
            return MercBaseDamage(merc) * CombinedBonuses.Get(BonusType.MULTIPLY_MERC_DMG, 1) * CombinedBonuses.Get(BonusType.MULTIPLY_ALL_DMG, 1) * CombinedBonuses.Get(merc.AttackType.Bonus(), 1);
        }
        #endregion

        public BigDouble TotalTapDamage
        {
            get
            {
                return CombinedBonuses.Get(BonusType.FLAT_TAP_DMG, 1) * CombinedBonuses.Get(BonusType.MULTIPLY_TAP_DMG, 1) * CombinedBonuses.Get(BonusType.MULTIPLY_ALL_DMG, 1);
            }
        }

        private Dictionary<BonusType, BigDouble> CreateBonusDictionary(IEnumerable<KeyValuePair<BonusType, BigDouble>> ls)
        {
            Dictionary<BonusType, BigDouble> result = new Dictionary<BonusType, BigDouble>();

            foreach (var pair in ls)
            {
                BonusType bonus = pair.Key;
                BigDouble value = pair.Value;

                if (!result.TryGetValue(bonus, out BigDouble totalValue))
                {
                    result[bonus] = value;
                }
                else
                {
                    result[bonus] = bonus switch
                    {
                        BonusType.FLAT_CRIT_CHANCE => totalValue + value,
                        BonusType.FLAT_TAP_DMG => totalValue + value,

                        _ => totalValue * value
                    };
                }
            }

            return result;
        }
    }
}