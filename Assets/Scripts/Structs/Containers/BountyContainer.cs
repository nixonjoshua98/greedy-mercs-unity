using System;

using System.Collections.Generic;

using SimpleJSON;


using BountyID = BountyData.BountyID;

public class BountyContainer
{
    Dictionary<BountyID, BountyState> bounties;

    public int Count { get { return bounties.Count; } }

    public BountyContainer(JSONNode node)
    {
        Update(node);
    }

    public void Update(JSONNode node)
    {
        bounties = new Dictionary<BountyID, BountyState>();

        foreach (JSONNode bounty in node["bounties"].AsArray)
        {
            Set((BountyID)int.Parse(bounty["bountyId"]), bounty["startTime"].AsLong);
        }
    }

    public JSONNode ToJson()
    {
        JSONArray array = new JSONArray();

        foreach (var bounty in bounties)
        {
            long startTimestamp = (new DateTimeOffset(bounty.Value.startTime)).ToUnixTimeMilliseconds();

            JSONNode node = new JSONObject();

            node.Add("startTime", startTimestamp);
            node.Add("bountyId", (int)bounty.Key);

            array.Add(node);
        }

        return array;
    }

    public bool TryGetBounty(BountyID bounty, out BountyState state)
    {
        return bounties.TryGetValue(bounty, out state);
    }

    public void Set(BountyID bounty, long startTime)
    {
        bounties[bounty] = new BountyState { startTime = DateTimeOffset.FromUnixTimeMilliseconds(startTime).DateTime };
    }

    public void Remove(BountyID bounty)
    {
        bounties.Remove(bounty);
    }
}