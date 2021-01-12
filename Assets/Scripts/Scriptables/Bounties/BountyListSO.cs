using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace BountyData
{
    public struct BountyStaticData
    {
        public int bountyPoints;
        public int unlockStage;
        public int maxLevel;
    }

    [PreferBinarySerialization]
    [CreateAssetMenu(menuName = "Scriptables/Container/BountyList")]
    public class BountyListSO : ScriptableObject
    {
        public List<BountySO> BountyList;

        Dictionary<BountyID, BountySO> BountyDict;

        public void Restore(SimpleJSON.JSONNode node)
        {
            BountyDict = new Dictionary<BountyID, BountySO>();

            foreach (string key in node.Keys)
            {
                BountyID id = (BountyID)int.Parse(key);

                BountyStaticData data = JsonUtility.FromJson<BountyStaticData>(node[key].ToString());

                BountySO bounty = GetFromList(id);

                bounty.Init(data);

                BountyDict.Add(bounty.BountyID, bounty);
            }

            Debug.Log("Restored Bounty Data");
        }

        // === Helper Methods ===

        public BountySO Get(BountyID bounty) => BountyDict[bounty];

        public bool GetStageBoss(int stage, out BountySO bounty)
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