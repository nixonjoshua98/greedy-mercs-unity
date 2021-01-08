using System;

using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;


public class BountyContainer
{
    const int MAX_HOURS = 12;

    DateTime lastClaimTime;

    public DateTime LastClaimTime {  get { return lastClaimTime; } }

    public BountyContainer(JSONNode node)
    {
        lastClaimTime = DateTime.UtcNow;

        Update(node);
    }

    public void Update(JSONNode node)
    {
        if (node.HasKey("bounties"))
            node = node["bounties"];

        lastClaimTime = node.HasKey("lastClaimTime") ? DateTimeOffset.FromUnixTimeMilliseconds(node["lastClaimTime"].AsLong).DateTime : lastClaimTime;
    }

    public JSONNode ToJson()
    {
        JSONNode node = new JSONObject();

        node.Add("lastClaimTime", lastClaimTime.ToUnixMilliseconds());

        return node;
    }

    // === Helper ===

    public Dictionary<BountyID, BountySO> Unlocked()
    {
        int stage = Mathf.Max(GameState.Player.maxPrestigeStage, GameState.Stage.stage);

        Dictionary<BountyID, BountySO> unlocked = new Dictionary<BountyID, BountySO>();

        foreach (BountySO bounty in StaticData.Bounties.List)
        {
            if (stage > bounty.unlockStage)
                unlocked.Add(bounty.BountyID, bounty);
        }

        return unlocked;
    }


    public int TimeSinceClaim
    {
        get
        {
            if (HourlyIncome == 0)
                lastClaimTime = DateTime.UtcNow;

            float secondsSinceClaim = (float)(DateTime.UtcNow - lastClaimTime).TotalSeconds;

            return Mathf.Max(0, Mathf.FloorToInt(Mathf.Min(MAX_HOURS * 3_600.0f, secondsSinceClaim)));
        }
    }

    public float PercentFilled { get { return TimeSinceClaim / (MAX_HOURS * 3_600.0f); } }

    public int CurrentClaimAmount { get { return Mathf.FloorToInt((TimeSinceClaim / 3_600.0f) * HourlyIncome); } }

    public int MaxClaimAmount {  get { return HourlyIncome * 12; } }

    public int HourlyIncome
    {
        get
        {
            int total = 0;

            foreach (var bounty in Unlocked())
                total += bounty.Value.bountyPoints;

            return total;
        }
    }
}