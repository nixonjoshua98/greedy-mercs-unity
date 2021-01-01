using System;

using BountyID      = BountyData.BountyID;
using CharacterID   = CharacterData.CharacterID;

// TODO: Move into CharacterData namespace

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
public class UpgradeState
{
    public int level = 1;
}

[System.Serializable]
public class BountyState
{
    public DateTime startTime;
}