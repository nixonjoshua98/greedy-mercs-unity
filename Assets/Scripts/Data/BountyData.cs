using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace BountyData
{
    public class BountyStaticData
    {
        public int bountyPoints;

        public int unlockStage;
    }

    public class Bounties
    {
        Dictionary<int, BountyStaticData> bounties;

        public Bounties(JSONNode node)
        {
            bounties = new Dictionary<int, BountyStaticData>();

            foreach (string key in node.Keys)
                bounties[int.Parse(key)] = JsonUtility.FromJson<BountyStaticData>(node[key].ToString());
        }

        public Dictionary<int, BountyStaticData> All() => bounties;
    }
}