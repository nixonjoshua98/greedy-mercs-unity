using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


// === Local Player Data
#region local


[System.Serializable]
public class PlayerState
{
    public double gold;

    public List<PlayerUpgradeState> upgrades = new List<PlayerUpgradeState>();

    public void OnRestored()
    {
        foreach (PlayerUpgradeID upgrade in Enum.GetValues(typeof(PlayerUpgradeID)))
        {
            if (!TryGetUpgrade(upgrade, out PlayerUpgradeState _))
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

    public bool TryGetUpgrade(PlayerUpgradeID playerUpgrade, out PlayerUpgradeState result)
    {
        result = GetUpgradeState(playerUpgrade);

        return result != null;
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
    public HeroID heroId = HeroID.ERROR;

    public int level = 1;

    public bool inSquad = false;
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
public class StaticServerHeroData
{
    public float baseCost;
}


[System.Serializable]
public class HeroPassiveSkill
{
    public BonusType type = BonusType.ERROR;

    public string name = "No Name";

    public double value = 1;
}


#endregion