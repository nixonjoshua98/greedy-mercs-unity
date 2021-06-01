using SimpleJSON;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GM.Bounty
{
    public struct BountyDataStruct
    {
        public int ID;

        public string Name;

        public int HourlyIncome;
        public int UnlockStage;

        public string _iconString;
        public string _prefabString;

        public Sprite Icon { get { return ResourceManager.LoadSprite(_iconString); } }
        public GameObject EnemyPrefab { get { return ResourceManager.LoadPrefab(_prefabString); } }
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
                JSONNode current = node[key];

                int id = int.Parse(key);

                BountyDataStruct data = new BountyDataStruct()
                {
                    ID = id,

                    Name = current["name"],

                    HourlyIncome    = current["hourlyIncome"].AsInt,
                    UnlockStage     = current["unlockStage"].AsInt,

                    _iconString     = current["iconString"],
                    _prefabString   = current["prefabString"],
                };

                bountyData.Add(id, data);
            }
        }

        // = = = Get Methods = = = //

        public BountyDataStruct Get(int bounty) { return bountyData[bounty]; }
        public bool Contains(int bounty) { return bountyData.ContainsKey(bounty); }

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