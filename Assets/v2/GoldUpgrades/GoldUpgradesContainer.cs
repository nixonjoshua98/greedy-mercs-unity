namespace GM.GoldUpgrades
{
    public class GoldUpgradesContainer
    {
        public GoldUpgradeState TapUpgrade = new() { Level = 1 };

        public void DeleteLocalStateData()
        {
            TapUpgrade.Level = 1;
        }
    }
}
