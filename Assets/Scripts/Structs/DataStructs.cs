using System;
using System.Collections;
using System.Collections.Generic;

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
    public double prestigePoints;

    public List<PlayerUpgradeState> upgrades = new List<PlayerUpgradeState>();

    public void OnRestored()
    {
        foreach (PlayerUpgradeID upgrade in Enum.GetValues(typeof(PlayerUpgradeID)))
        {
            if (GetUpgradeState(upgrade) == null)
            {
                upgrades.Add(new PlayerUpgradeState { upgradeId = upgrade });
            }
        }
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
    public PlayerUpgradeID upgradeId = PlayerUpgradeID.ERROR;

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