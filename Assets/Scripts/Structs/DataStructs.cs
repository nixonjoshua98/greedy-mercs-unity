using System;
using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;


// === Local Player Data
#region local

public class CharacterStaticData
{
    public readonly CharacterID HeroID;
    public readonly BonusType AttackType;

    public readonly string Name;
    public readonly BigDouble PurchaseCost;
    public readonly string GameObjectString;

    public CharacterStaticData(CharacterID heroId, BonusType attackType, string name, string gameObjectString, BigDouble purchaseCost)
    {
        HeroID              = heroId;

        AttackType          = attackType;

        Name                = name;
        PurchaseCost        = purchaseCost;
        GameObjectString    = gameObjectString;
    }
}


[System.Serializable]
public class RelicStaticData
{
    public string name;

    public int maxLevel = 1_000;

    public string description;

    public BonusType bonusType;

    public int baseCost;
    public float costPower;

    public float baseEffect;
    public float levelEffect;
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

    public string name;
    public string description = "";

    public double value = 1;
}


#endregion