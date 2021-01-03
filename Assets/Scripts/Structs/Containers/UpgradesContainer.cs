using System;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

public class UpgradesContainer
{
    Dictionary<UpgradeID, UpgradeState> upgrades;

    public UpgradesContainer(JSONNode node)
    {
        upgrades = new Dictionary<UpgradeID, UpgradeState>();

        foreach (JSONNode upgrade in node["upgrades"].AsArray)
            upgrades[(UpgradeID)int.Parse(upgrade["upgradeId"])] = JsonUtility.FromJson<UpgradeState>(upgrade.ToString());

        if (!upgrades.ContainsKey(UpgradeID.TAP_DAMAGE))    AddUpgrade(UpgradeID.TAP_DAMAGE, 1);
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

    public void AddUpgrade(UpgradeID playerUpgrade, int level = 1)
    {
        upgrades[playerUpgrade] = new UpgradeState { level = level };
    }
}
