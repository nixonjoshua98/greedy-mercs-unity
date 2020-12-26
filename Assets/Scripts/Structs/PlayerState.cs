using System;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;


[System.Serializable]
public class PlayerState
{
    public double gold;

    public ulong prestigePoints;

    public Dictionary<PlayerUpgradeID, UpgradeState> upgrades;

    public void Restore(JSONNode node)
    {
        upgrades = CreateUpgradesDictionary(node);

        Update(node);
    }

    public void Update(JSONNode node)
    {
        prestigePoints = node.HasKey("prestigePoints") ? ulong.Parse(node["prestigePoints"]) : 0;
    }

    public JSONNode ToJson()
    {
        JSONNode node = JSON.Parse(JsonUtility.ToJson(this));

        node["upgrades"]        = Utils.Json.CreateJSONArray("upgradeId", upgrades);
        node["prestigePoints"]  = prestigePoints.ToString();

        return node;
    }

    public UpgradeState GetUpgrade(PlayerUpgradeID playerUpgrade)
    {
        return upgrades[playerUpgrade];
    }

    static Dictionary<PlayerUpgradeID, UpgradeState> CreateUpgradesDictionary(JSONNode node)
    {
        Dictionary<PlayerUpgradeID, UpgradeState> upgrades = new Dictionary<PlayerUpgradeID, UpgradeState>();

        foreach (JSONNode upgrade in node["upgrades"].AsArray)
            upgrades[(PlayerUpgradeID)int.Parse(upgrade["upgradeId"])] = JsonUtility.FromJson<UpgradeState>(upgrade.ToString());

        foreach (PlayerUpgradeID upgrade in Enum.GetValues(typeof(PlayerUpgradeID)))
        {
            if (!upgrades.ContainsKey(upgrade))
                upgrades[upgrade] = new UpgradeState { level = 1 };
        }

            return upgrades;
    }

}