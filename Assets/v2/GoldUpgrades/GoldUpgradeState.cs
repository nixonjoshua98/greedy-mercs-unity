namespace GM.GoldUpgrades
{
    public class GoldUpgradeState
    {
        public int Level = 0;
    }


    public class TapDamageGoldUpgrade : GM.Core.GMClass
    {
        public int Level = 0;

        public const int MaxLevel_ = 1_000;
        public bool IsMaxLevel => Level >= MaxLevel_;

        public BigDouble UpgradeCost(int levels) => App.GMCache.TapUpgradeCost(levels);
    }
}
