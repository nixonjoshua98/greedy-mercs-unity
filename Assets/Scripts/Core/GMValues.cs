using GM.Armoury;
using GM.Common;
using GM.Enums;
using GM.Mercs.Data;
using System.Collections.Generic;
using System.Linq;

namespace GM.Core
{
    public class GMValues : GMClass
    {
        private List<KeyValuePair<BonusType, BigDouble>> MercPassiveBonuses
        {
            get
            {
                List<KeyValuePair<BonusType, BigDouble>> ls = new();

                App.Mercs.UnlockedMercs.ForEach(merc =>
                {
                    ls.AddRange(merc.UnlockedPassives.Select(x => new KeyValuePair<BonusType, BigDouble>(x.BonusType, x.BonusValue)));
                });

                return ls;
            }
        }

        private IEnumerable<KeyValuePair<BonusType, BigDouble>> ArtefactBonuses =>
            App.Artefacts.UserOwnedArtefacts.Select(s => new KeyValuePair<BonusType, BigDouble>(s.Bonus, s.Effect));

        private IEnumerable<KeyValuePair<BonusType, BigDouble>> ArmouryBonuses =>
            App.Armoury.UserItems.Select(x => new KeyValuePair<BonusType, BigDouble>(x.BonusType, x.BonusValue));

        private IEnumerable<KeyValuePair<BonusType, BigDouble>> BonusesFromBounties =>
            App.Bounties.UnlockedBounties.Where(x => x.Level > 0).Select(x => new KeyValuePair<BonusType, BigDouble>(x.BonusType, x.BonusValue));

        private Dictionary<BonusType, BigDouble> CombinedBonuses
        {
            get
            {
                var ls = MercPassiveBonuses
                    .Concat(ArtefactBonuses)
                    .Concat(BonusesFromBounties)
                    .Concat(ArmouryBonuses);

                return CreateBonusDictionary(ls);
            }
        }

        public BigDouble GetResolvedBonus(BonusType bonus)
        {
            return bonus switch
            {
                BonusType.FLAT_CRIT_CHANCE => CombinedBonuses.Get(bonus, 0),
                _ => CombinedBonuses.Get(bonus, 1)
            };
        }

        #region Prestige Points

        public double BasePrestigePoints()
        {
            return GameFormulas.BasePrestigePoints(App.GameState.Stage);
        }

        public double BasePrestigePoints(int stage)
        {
            return GameFormulas.BasePrestigePoints(stage);
        }

        public double BonusPrestigePoints()
        {
            return BonusPrestigePoints(App.GameState.Stage);
        }

        public double BonusPrestigePoints(int stage)
        {
            BigDouble multiplier = GetResolvedBonus(BonusType.MULTIPLY_PRESTIGE_BONUS);

            var basePoints = GameFormulas.BasePrestigePoints(stage);

            return (multiplier > 1 ? basePoints * (multiplier - 1) : 0).ToDouble();
        }

        #endregion

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

        #region Critical Hit
        public float CriticalHitChance => (float)(Constants.BASE_CRIT_CHANCE + CombinedBonuses.Get(BonusType.FLAT_CRIT_CHANCE, 0)).ToDouble();

        public BigDouble CriticalHitMultiplier => Constants.BASE_CRIT_MULTIPLIER + CombinedBonuses.Get(BonusType.MULTIPLY_CRIT_DMG, 1);
        #endregion

        #region Tap Upgrade
        public BigDouble TapUpgradeCost(int levels)
        {
            return GameFormulas.TapUpgradeUpgradeCost(App.GoldUpgrades.TapUpgrade.Level, levels);
        }

        public BigDouble TapUpgradeDamage => GameFormulas.TapUpgradeDamage(App.GoldUpgrades.TapUpgrade.Level);
        #endregion

        #region Artefacts
        public BigDouble ArtefactBaseEffect(Artefacts.Data.AggregatedArtefactData art)
        {
            return GameFormulas.ArtefactBonusValue(art.CurrentLevel, art.BaseEffect, art.LevelEffect);
        }

        public BigDouble ArtefactEffect(Artefacts.Data.AggregatedArtefactData art)
        {
            return ArtefactBaseEffect(art);
        }

        public double ArtefactUnlockCost(int owned)
        {
            return GameFormulas.ArtefactUnlockCost(owned);
        }

        public double ArtefactUpgradeCost(int artefactId, int levels)
        {
            return ArtefactUpgradeCost(App.Artefacts.GetArtefact(artefactId), levels);
        }

        public double ArtefactUpgradeCost(GM.Artefacts.Data.AggregatedArtefactData data, int levels)
        {
            return GameFormulas.ArtefactUpgradeCost(data.CurrentLevel, levels, data.CostExpo, data.CostCoeff);
        }
        #endregion

        #region Armoury
        public double ArmouryItemBonusValue(AggregatedArmouryItem item)
        {
            return GameFormulas.ArmouryItemBonusValue(item.CurrentLevel, item.NumOwned, item.BaseEffect, item.LevelEffect);
        }

        public int ArmouryItemUpgradeCost(AggregatedArmouryItem item)
        {
            return GameFormulas.ArmouryItemUpgradeCost(item.CurrentLevel);
        }
        #endregion

        /// <summary>Upgrade cost for merc. CurrentLevel -> (CurrentLevel + levels)</summary>
        public BigDouble MercUpgradeCost(GM.Mercs.Data.AggregatedMercData merc, int levels)
        {
            return GameFormulas.MercUpgradeCost(merc.CurrentLevel, levels);
        }

        /// <summary>
        /// Base merc damage for current level. Does not apply any bonuses
        /// </summary>
        public BigDouble MercBaseDamage(AggregatedMercData merc) => GameFormulas.MercBaseDamageAtLevel(merc.BaseDamage, merc.CurrentLevel);

        /// <summary>
        /// Base merc damage for a given level. Does not apply any bonuses
        /// </summary>
        public BigDouble MercBaseDamage(AggregatedMercData merc, int level) => GameFormulas.MercBaseDamageAtLevel(merc.BaseDamage, level);

        public BigDouble MercDamagePerAttack(AggregatedMercData merc)
        {
            return MercBaseDamage(merc) * 
                CombinedBonuses.Get(BonusType.MULTIPLY_MERC_DMG, 1) *
                CombinedBonuses.Get(BonusType.MULTIPLY_ALL_DMG, 1) * 
                CombinedBonuses.Get(merc.AttackType.Bonus(), 1);
        }

        public BigDouble MercDamagePerAttack(AggregatedMercData merc, int level)
        {
            return MercBaseDamage(merc, level) *
                CombinedBonuses.Get(BonusType.MULTIPLY_MERC_DMG, 1) *
                CombinedBonuses.Get(BonusType.MULTIPLY_ALL_DMG, 1) *
                CombinedBonuses.Get(merc.AttackType.Bonus(), 1);
        }

        public BigDouble TotalTapDamage => TapUpgradeDamage * CombinedBonuses.Get(BonusType.MULTIPLY_TAP_DMG, 1) * CombinedBonuses.Get(BonusType.MULTIPLY_ALL_DMG, 1);

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

                        _ => totalValue * value
                    };
                }
            }

            return result;
        }
    }
}