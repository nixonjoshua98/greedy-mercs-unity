using GM.Armoury.Data;
using GM.Common;
using GM.Common.Enums;
using GM.Mercs;
using GM.Mercs.Data;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace GM.Core
{
    public class GMCache : GMClass
    {
        List<KeyValuePair<BonusType, BigDouble>> MercPassiveBonuses
        {
            get
            {
                List<KeyValuePair<BonusType, BigDouble>> ls = new List<KeyValuePair<BonusType, BigDouble>>();

                App.Mercs.MercsInSquad.ForEach(mercId =>
                {
                    AggregatedMercData merc = App.Mercs.GetMerc(mercId);

                    IEnumerable<MercPassive> passives = merc.Passives.Where(x => merc.CurrentLevel >= x.UnlockLevel).Select(m => m.Values);

                    ls.AddRange(passives.Select(x => new KeyValuePair<BonusType, BigDouble>(x.Type, x.Value)));
                });

                return ls;
            }
        }
        IEnumerable<KeyValuePair<BonusType, BigDouble>> ArtefactBonuses => App.Artefacts.UserOwnedArtefacts.Select(s => new KeyValuePair<BonusType, BigDouble>(s.Bonus, s.Effect));
        IEnumerable<KeyValuePair<BonusType, BigDouble>> ArmouryBonuses => App.Armoury.UserItems.Select(x => new KeyValuePair<BonusType, BigDouble>(x.BonusType, x.BonusValue));

        Dictionary<BonusType, BigDouble> CombinedBonuses
        {
            get
            {
                var ls = MercPassiveBonuses
                    .Concat(ArtefactBonuses)
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

        #region Tap Upgrade
        public BigDouble TapUpgradeCost(int levels) => GameFormulas.TapUpgradeUpgradeCost(App.GoldUpgrades.TapUpgrade.Level, levels);
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

        public double ArtefactUnlockCost(int owned) => GameFormulas.ArtefactUnlockCost(owned);

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
        public double ArmouryItemBonusValue(ArmouryItemData item)
        {
            return GameFormulas.ArmouryItemBonusValue(item.CurrentLevel, item.NumOwned, item.BaseEffect, item.LevelEffect);
        }

        public int ArmouryItemUpgradeCost(ArmouryItemData item)
        {
            return GameFormulas.ArmouryItemUpgradeCost(item.CurrentLevel);
        }
        #endregion

        #region Mercs
        /// <summary>Upgrade cost for merc. CurrentLevel -> (CurrentLevel + levels)</summary>
        public BigDouble MercUpgradeCost(GM.Mercs.Data.AggregatedMercData merc, int levels)
        {
            return GameFormulas.MercUpgradeCost(merc.CurrentLevel, levels);
        }

        /// <summary>Base merc damage for current level. Does not apply any bonuses</summary>
        public BigDouble MercBaseDamage(GM.Mercs.Data.AggregatedMercData merc)
        {
            return GameFormulas.MercBaseDamageAtLevel(merc.BaseDamage, merc.CurrentLevel);
        }

        public BigDouble MercDamagePerAttack(GM.Mercs.Data.AggregatedMercData merc)
        {
            return MercBaseDamage(merc) * CombinedBonuses.Get(BonusType.MULTIPLY_MERC_DMG, 1) * CombinedBonuses.Get(BonusType.MULTIPLY_ALL_DMG, 1) * CombinedBonuses.Get(merc.AttackType.Bonus(), 1);
        }
        #endregion

        public BigDouble TotalTapDamage
        {
            get
            {
                return TapUpgradeDamage * CombinedBonuses.Get(BonusType.MULTIPLY_TAP_DMG, 1) * CombinedBonuses.Get(BonusType.MULTIPLY_ALL_DMG, 1);
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

                        _ => totalValue * value
                    };
                }
            }

            return result;
        }
    }
}