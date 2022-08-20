using SRC.Armoury;
using SRC.Artefacts.Data;
using SRC.Common;
using SRC.Common.Enums;
using SRC.Mercs.Data;

namespace SRC.Core
{
    public sealed class Calculations : GMClass
    {
        public static Calculations Instance = new();

        public double GetBonus(BonusType bonusType) => ResolvedBonusDictionary.GetBonus(bonusType);

        #region Prestige Points
        public double PrestigePointsAtStage(int stage)
        {
            return GameFormulas.PrestigePoints(stage) * 
                GetBonus(BonusType.MULTIPLY_PRESTIGE_BONUS);
        }
        public double BonusPrestigePointsAtStage(int stage)
        {
            BigDouble multiplier = GetBonus(BonusType.MULTIPLY_PRESTIGE_BONUS);

            var basePoints = GameFormulas.PrestigePoints(stage);

            return (multiplier > 1 ? basePoints * (multiplier - 1) : 0).ToDouble();
        }
        #endregion

        #region Stage Enemies Health
        public BigDouble StageEnemyHealth(int stage) => GameFormulas.EnemyHealth(stage);
        public BigDouble StageBossHealth(int stage) => GameFormulas.BossHealth(stage);
        #endregion

        #region Stage Enemies Gold
        public BigDouble GoldPerStageEnemy(int stage)
        {
            return GameFormulas.CalcEnemyGold(stage) * 
                GetBonus(BonusType.MULTIPLY_ENEMY_GOLD) * 
                GetBonus(BonusType.MULTIPLY_ALL_GOLD);
        }
        public BigDouble GoldPerStageBoss(int stage)
        {
            return GameFormulas.CalcBossGold(stage) * 
                GetBonus(BonusType.MULTIPLY_BOSS_GOLD) * 
                GetBonus(BonusType.MULTIPLY_ALL_GOLD);
        }
        #endregion

        #region Artefact Bonuses
        public double ArtefactBonus(AggregatedArtefactData art)
        {
            return GameFormulas.ArtefactBonus(art.CurrentLevel, art.BaseEffect, art.LevelEffect);
        }
        #endregion

        #region Artefact Unlock
        public double ArtefactUnlockCost(int owned) => GameFormulas.ArtefactUnlockCost(owned);
        #endregion

        #region Artefact Upgrades
        public double ArtefactUpgradeCost(Artefacts.Data.AggregatedArtefactData data, int levels)
        {
            return GameFormulas.ArtefactUpgradeCost(data.CurrentLevel, levels, data.CostExpo, data.CostCoeff);
        }
        #endregion

        #region Critical Hit
        public double CriticalHitChance => Constants.BASE_CRIT_CHANCE + GetBonus(BonusType.FLAT_CRIT_CHANCE);
        public double CriticalHitMultiplier => Constants.BASE_CRIT_MULTIPLIER + GetBonus(BonusType.MULTIPLY_CRIT_DMG);
        #endregion

        #region Armoury
        public double ArmouryItemBonus(AggregatedArmouryItem item)
        {
            return GameFormulas.ArmouryItemBonus(item.CurrentLevel, item.NumOwned, item.BaseEffect, item.LevelEffect);
        }

        public int ArmouryItemUpgradeCost(AggregatedArmouryItem item)
        {
            return GameFormulas.ArmouryItemUpgradeCost(item.CurrentLevel);
        }
        #endregion

        #region Merc Upgrades
        public BigDouble MercUpgradeCost(AggregatedMercData merc, int levels) => GameFormulas.MercUpgradeCost(merc.CurrentLevel, levels);
        #endregion

        #region Merc Damage
        public BigDouble MercBaseDamage(AggregatedMercData merc, int level) => GameFormulas.MercBaseDamageAtLevel(merc.BaseDamage, level);
        public BigDouble MercDamagePerAttack(AggregatedMercData merc) => MercDamagePerAttack(merc, merc.CurrentLevel);
        public BigDouble MercDamagePerAttack(AggregatedMercData merc, int level)
        {
            return MercBaseDamage(merc, level) *
                GetBonus(BonusType.MULTIPLY_MERC_DMG) *
                GetBonus(BonusType.MULTIPLY_ALL_DMG) *
                GetBonus(merc.AttackType.Bonus());
        }
        #endregion

        #region Tap Upgrade
        public BigDouble TapUpgradeCost(int levels)
        {
            return GameFormulas.TapUpgradeUpgradeCost(App.GoldUpgrades.TapUpgrade.Level, levels);
        }
        public BigDouble TapUpgradeDamage => GameFormulas.TapUpgradeDamage(App.GoldUpgrades.TapUpgrade.Level);
        #endregion

        #region Tap Damage
        public BigDouble TotalTapDamage => TapUpgradeDamage * GetBonus(BonusType.MULTIPLY_TAP_DMG) * GetBonus(BonusType.MULTIPLY_ALL_DMG);
        #endregion
    }
}
