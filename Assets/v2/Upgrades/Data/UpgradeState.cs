using GM.Common.Enums;

namespace GM.Upgrades.Data
{
    public class UpgradeState : Core.GMClass
    {
        public UpgradeID Id = UpgradeID.NONE;

        public int Level;
        public readonly int MaxLevel;

        public bool IsMaxLevel => Level >= MaxLevel;
        public BonusType BonusType { get; private set; } = BonusType.NONE;

        public UpgradeState(int level, int maxLevel, BonusType bonus)
        {
            Level = level;
            MaxLevel = maxLevel;
            BonusType = bonus;
        }

        public BigDouble Value
        {
            get
            {
                return Id switch
                {
                    UpgradeID.FLAT_TAP_DMG => App.Cache.MinorTapUpgradeDamage,
                    UpgradeID.MULT_TAP_DMG => App.Cache.MajorTapUpgradeDamage,
                    _ => throw new System.Exception()
                };
            }
        }
    }
}