using System;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

public class PlayerUpgradesContainer
{
    Dictionary<UpgradeID, UpgradeState> upgrades;

    public PlayerUpgradesContainer(JSONNode node)
    {
        upgrades = new Dictionary<UpgradeID, UpgradeState>();

        foreach (JSONNode upgrade in node["playerUpgrades"].AsArray)
        {
            upgrades[(UpgradeID)int.Parse(upgrade["upgradeId"])] = JsonUtility.FromJson<UpgradeState>(upgrade.ToString());
        }

        foreach (UpgradeID upgrade in Enum.GetValues(typeof(UpgradeID)))
        {
            if (!upgrades.ContainsKey(upgrade))
                AddUpgrade(upgrade);
        }
    }

    public JSONNode ToJson()
    {
        return Utils.Json.CreateJSONArray("upgradeId", upgrades);
    }

    // === Helper Methods ===

    public UpgradeState GetUpgrade(UpgradeID playerUpgrade)
    {
        return upgrades[playerUpgrade];
    }

    public bool TryGetUpgrade(UpgradeID playerUpgrade, out UpgradeState state)
    {
        return upgrades.TryGetValue(playerUpgrade, out state);
    }

    public void AddUpgrade(UpgradeID playerUpgrade)
    {
        upgrades[playerUpgrade] = new UpgradeState { level = 1 };
    }
}
