using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace GreedyMercs
{
    public struct BountyServerData
    {
        public int hourlyIncome;
        public int unlockStage;
        public int maxLevel;

        public string iconString;
        public string prefabString;
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Containers/BountyList")]
    public class BountyListSO : ScriptableObject
    {
        public List<BountySO> BountyList;

        Dictionary<BountyID, BountySO> BountyDict;

        public void Init(SimpleJSON.JSONNode node)
        {
            node = node["bounties"];

            BountyDict = new Dictionary<BountyID, BountySO>();

            foreach (string key in node.Keys)
            {
                BountyID id = (BountyID)int.Parse(key);

                BountyServerData data = JsonUtility.FromJson<BountyServerData>(node[key].ToString());

                BountySO bounty = GetFromList(id);

                bounty.Init(data);

                BountyDict.Add(bounty.BountyID, bounty);
            }
        }

        // === Helper Methods ===

        public BountySO Get(BountyID bounty) => BountyDict[bounty];
        public BountySO Get(int bounty) => Get((BountyID)bounty);

        public bool IsBountyBoss(int stage, out BountySO bounty)
        {
            bounty = null;

            foreach (BountySO b in BountyList)
            {
                bounty = b;

                if (b.unlockStage == stage)
                    return true;
            }

            return false;
        }

        // === Private Methods ===

        BountySO GetFromList(BountyID id)
        {
            foreach (BountySO b in BountyList)
            {
                if (b.BountyID == id)
                    return b;
            }

            return null;
        }
    }
}