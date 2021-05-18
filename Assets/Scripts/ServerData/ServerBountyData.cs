using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Bounty
{
    public struct ServerBountyData
    {
        public readonly int BountyID;

        public string name;

        public int hourlyIncome;
        public int unlockStage;

        string iconString;
        string prefabString;

        public ServerBountyData(int _bountyId, JSONNode node)
        {
            BountyID = _bountyId;

            name = node["name"].Value;

            hourlyIncome    = node["hourlyIncome"].AsInt;
            unlockStage     = node["unlockStage"].AsInt;

            iconString      = node["iconString"].Value;
            prefabString    = node["prefabString"].Value;
        }

        public Sprite icon { get { return ResourceManager.LoadSprite("BountyIcons", iconString); } }
        public GameObject prefab { get { return ResourceManager.LoadPrefab("BountyBossPrefabs", prefabString); } }
    }

    public class ServerBountyDataController
    {
        Dictionary<int, ServerBountyData> bountyData;

        public readonly float MaxUnclaimedHours;

        public ServerBountyDataController(JSONNode node)
        {
            bountyData = new Dictionary<int, ServerBountyData>();

            MaxUnclaimedHours = node["maxUnclaimedHours"].AsFloat;

            SetupBountyData(node);
        }

        void SetupBountyData(JSONNode node)
        {
            node = node["bounties"];

            bountyData = new Dictionary<int, ServerBountyData>();

            foreach (string key in node.Keys)
            {
                int id = int.Parse(key);

                ServerBountyData data = new ServerBountyData(id, node[key]);

                bountyData.Add(id, data);
            }
        }

        public ServerBountyData Get(int bounty) => bountyData[bounty];

        public bool IsBountyBoss(int stage, out ServerBountyData bounty)
        {
            bounty = new ServerBountyData();

            foreach (ServerBountyData b in bountyData.Values)
            {
                bounty = b;

                if (b.unlockStage == stage)
                    return true;
            }

            return false;
        }
    }
}