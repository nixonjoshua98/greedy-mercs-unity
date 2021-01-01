using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SimpleJSON;

namespace BountyData
{
    public enum BountyID
    {
        BOUNTY_ONE = 0,
        BOUNTY_TWO = 1,
    }

    public class BountyStaticData
    {
        public string name;

        public int duration;
        public int bountyReward;
    }

    public class Bounties
    {
        Dictionary<BountyID, BountyStaticData> bounties;

        public Bounties(JSONNode node)
        {
            bounties = new Dictionary<BountyID, BountyStaticData>();

            foreach (string key in node.Keys)
                bounties[(BountyID)int.Parse(key)] = JsonUtility.FromJson<BountyStaticData>(node[key].ToString());
        }

        public BountyStaticData Get(BountyID bounty)
        {
            return bounties[bounty];
        }
    }
}