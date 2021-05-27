using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Bounty
{
    public struct BountyDataStruct
    {
        public readonly int ID;

        public readonly string Name;

        public readonly int HourlyIncome;
        public readonly int UnlockStage;

        string iconString;
        string prefabString;

        public BountyDataStruct(int _bountyId, JSONNode node)
        {
            ID = _bountyId;

            Name = node["name"].Value;

            HourlyIncome    = node["hourlyIncome"].AsInt;
            UnlockStage     = node["unlockStage"].AsInt;

            iconString      = node["iconString"].Value;
            prefabString    = node["prefabString"].Value;
        }

        public Sprite Icon { get { return ResourceManager.LoadSprite(iconString); } }
        public GameObject EnemyPrefab { get { return ResourceManager.LoadPrefab("BountyBossPrefabs", prefabString); } }
    }

    public class ServerBountyData
    {
        Dictionary<int, BountyDataStruct> bountyData;

        public readonly float MaxUnclaimedHours;

        public ServerBountyData(JSONNode node)
        {
            bountyData = new Dictionary<int, BountyDataStruct>();

            MaxUnclaimedHours = node["maxUnclaimedHours"].AsFloat;

            SetupBountyData(node);
        }

        void SetupBountyData(JSONNode node)
        {
            node = node["bounties"];

            bountyData = new Dictionary<int, BountyDataStruct>();

            foreach (string key in node.Keys)
            {
                int id = int.Parse(key);

                BountyDataStruct data = new BountyDataStruct(id, node[key]);

                bountyData.Add(id, data);
            }
        }

        public BountyDataStruct Get(int bounty) => bountyData[bounty];

        public bool IsBountyBoss(int stage, out BountyDataStruct bounty)
        {
            bounty = new BountyDataStruct();

            foreach (BountyDataStruct b in bountyData.Values)
            {
                bounty = b;

                if (b.UnlockStage == stage)
                    return true;
            }

            return false;
        }
    }
}