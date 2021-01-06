using System;

using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;


public class BountyContainer
{
    const int MAX_HOURS = 12;

    DateTime lastClaimTime;

    public BountyContainer(JSONNode node)
    {
        Update(node);
    }

    public void Update(JSONNode node)
    {
        if (node.HasKey("bounties"))
            node = node["bounties"];

        lastClaimTime = DateTimeOffset.FromUnixTimeMilliseconds(node["lastClaimTime"].AsLong).DateTime;
    }

    public JSONNode ToJson()
    {
        JSONNode node = new JSONObject();

        node.Add("lastClaimTime", (new DateTimeOffset(lastClaimTime)).ToUnixTimeMilliseconds());

        return node;
    }

    // === Helper ===
    public int TimeSinceClaim
    {
        get
        {
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

            foreach (var bounty in StaticData.Bounties.All())
            {
                total += bounty.Value.bountyPoints;
            }

            return total;
        }
    }
}