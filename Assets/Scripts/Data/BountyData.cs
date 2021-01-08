using SimpleJSON;
using System.Collections.Generic;

using UnityEngine;

namespace BountyData
{
    public struct ServerBountyData
    {
        public int bountyPoints;
        public int unlockStage;
    }

    public class Bounties
    {
        Dictionary<int, ServerBountyData> bounties;

        public Bounties(JSONNode node)
        {
            bounties = new Dictionary<int, ServerBountyData>();

            foreach (string key in node.Keys)
                bounties[int.Parse(key)] = JsonUtility.FromJson<ServerBountyData>(node[key].ToString());
        }

        public ServerBountyData Get(int bountyKey) => bounties[bountyKey];
    }
}