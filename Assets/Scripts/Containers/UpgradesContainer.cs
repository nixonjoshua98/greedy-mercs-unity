using System;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GreedyMercs
{
    public class UpgradesContainer
    {
        Dictionary<GoldUpgradeID, UpgradeState> upgrades;

        public UpgradesContainer(JSONNode node)
        {
            upgrades = new Dictionary<GoldUpgradeID, UpgradeState>();

            foreach (JSONNode upgrade in node["upgrades"].AsArray)
                upgrades[(GoldUpgradeID)int.Parse(upgrade["upgradeId"])] = JsonUtility.FromJson<UpgradeState>(upgrade.ToString());
        }

        public JSONNode ToJson()
        {
            return Utils.Json.CreateJSONArray("upgradeId", upgrades);
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