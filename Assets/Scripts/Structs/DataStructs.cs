using System;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;


// === Local Player Data
#region local

public class CharacterStaticData
{
    public readonly CharacterID HeroID;

    public readonly string Name;
    public readonly double PurchaseCost;
    public readonly string GameObjectString;

    public CharacterStaticData(CharacterID heroId, string name, string gameObjectString, double purchaseCost)
    {
        HeroID              = heroId;

        Name                = name;
        PurchaseCost        = purchaseCost;
        GameObjectString    = gameObjectString;
    }
}


[System.Serializable]
public class RelicStaticData
{
    public string name;

    public string description;

    public BonusType bonusType;

    public double baseCost;
    public double costPower;

    public double baseEffect;
    public double levelEffect;
}

[System.Serializable]
public class UpgradeState
{
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
    public BonusType bonusType;

    public string name = "No Name";

    public double value = 1;
}


#endregion