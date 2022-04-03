using GM.Common.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Bounties.Models
{
    // User Models

    public class UserBounty
    {
        public int BountyID;
        public int NumDefeats;
        public int Level;
    }

    public class UserBounties
    {
        public DateTime LastClaimTime;
        public List<UserBounty> UnlockedBounties;
    }

    // Game Models

    public class BountyLevel
    {
        public int NumDefeatsRequired;

        public float BonusValue;
    }

    public class Bounty
    {
        [JsonProperty(PropertyName = "BountyID")]
        public int ID;

        public string Name;
        public int HourlyIncome;
        public BonusType BonusType;
        public string Description;
        public int UnlockStage;
        public List<BountyLevel> Levels = new();

        [JsonIgnore]
        public Sprite Icon;
        [JsonIgnore]
        public GameObject Prefab;
    }

    public class BountiesDataModel
    {
        public float MaxUnclaimedHours;

        public List<Bounty> Bounties;
    }
}
