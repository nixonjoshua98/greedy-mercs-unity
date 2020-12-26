using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

public class PlayerUpgradesContainer : IContainer
{
    Dictionary<UpgradeID, UpgradeState> upgrades;

    public PlayerUpgradesContainer(JSONNode node)
    {
        upgrades = new Dictionary<UpgradeID, UpgradeState>();

        foreach (JSONNode chara in node["playerUpgrades"].AsArray)
        {
            upgrades[(UpgradeID)int.Parse(chara["upgradeId"])] = JsonUtility.FromJson<UpgradeState>(chara.ToString());
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
