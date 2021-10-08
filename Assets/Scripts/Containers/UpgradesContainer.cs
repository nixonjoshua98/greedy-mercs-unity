using System.Collections.Generic;

namespace GM
{
    public class UpgradesContainer
    {
        Dictionary<GoldUpgradeID, UpgradeState> upgrades;

        public UpgradesContainer()
        {
            upgrades = new Dictionary<GoldUpgradeID, UpgradeState>();
        }

        // === Helper Methods ===

        public UpgradeState GetUpgrade(GoldUpgradeID upgrade)
        {
            if (!upgrades.ContainsKey(upgrade))
            {
                AddUpgrade(upgrade, 1);
            }

            return upgrades[upgrade];
        }

        public void AddUpgrade(GoldUpgradeID playerUpgrade, int level = 1)
        {
            upgrades[playerUpgrade] = new UpgradeState { level = level };
        }
    }
}