using System;
using System.Numerics;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;


// === Local Player Data
#region local

public class HeroStaticData
{
    public readonly HeroID HeroID;

    public readonly string Name;
    public readonly double PurchaseCost;
    public readonly string GameObjectString;

    public HeroStaticData(HeroID heroId, string name, string gameObjectString, double purchaseCost)
    {
        HeroID              = heroId;

        Name                = name;
        PurchaseCost        = purchaseCost;
        GameObjectString    = gameObjectString;
    }
}


[System.Serializable]
public class PlayerState
{
    public double gold;

    public BigInteger prestigePoints;

    public List<PlayerUpgradeState> upgrades;

    public void Restore(JSONNode node)
    {
        upgrades = new List<PlayerUpgradeState>();

        foreach (PlayerUpgradeID upgrade in Enum.GetValues(typeof(PlayerUpgradeID)))
        {
            if (GetUpgradeState(upgrade) == null)
                upgrades.Add(new PlayerUpgradeState { upgradeId = upgrade });
        }

        prestigePoints = BigInteger.Parse(node["prestigePoints"]);
    }

    // === Helper Methods ===

    public JSONNode ToJson()
    {
        JSONNode node = JSON.Parse(JsonUtility.ToJson(this));

        node["prestigePoints"] = prestigePoints.ToString();

        return node;
    }

    public PlayerUpgradeState GetUpgradeState(PlayerUpgradeID playerUpgrade)
    {
        foreach (PlayerUpgradeState state in upgrades)
        {
            if (state.upgradeId == playerUpgrade)
            {
                return state;
            }
        }

        return null;
    }
}


[System.Serializable]
public class PlayerUpgradeState
{
    public PlayerUpgradeID upgradeId;

    public int level = 1;
}


[System.Serializable]
public class HeroState
{
    public HeroID heroId;

    public int level = 1;
}


#endregion

// === Static Data ===

#region static

[System.Serializable]
public class HeroPassiveUnlock
{
    public int skill;

    public int unlockLevel;
}


[System.Serializable]
public class HeroPassiveSkill
{
    public BonusType type;

    public string name = "No Name";

    public double value = 1;
}


#endregion