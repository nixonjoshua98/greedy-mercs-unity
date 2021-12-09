using GM.Common.Enums;

namespace GM.Upgrades.Data
{
    public class UpgradeState : Core.GMClass
    {
        public UpgradeID Id = UpgradeID.NONE;

        public int Level;
        public readonly int MaxLevel;

        public IUpgradeRequirement UnlockRequirement { get; private set; }

        public bool IsMaxLevel => Level >= MaxLevel;
        public BonusType BonusType { get; private set; } = BonusType.NONE;

        public UpgradeState(int level, int maxLevel, BonusType bonus)
        {
            Level = level;
            MaxLevel = maxLevel;
            BonusType = bonus;
            UnlockRequirement = DefaultUpgradeRequirement.Value;
        }

        public UpgradeState(int level, int maxLevel, BonusType bonus, IUpgradeRequirement requirement)
        {
            Level = level;
            MaxLevel = maxLevel;
            BonusType = bonus;
            UnlockRequirement = requirement;
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