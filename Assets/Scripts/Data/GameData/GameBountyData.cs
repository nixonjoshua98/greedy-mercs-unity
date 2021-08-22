using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;

namespace GM.Bounty
{
    public struct BountyData
    {
        public int Id;
        public string Name;

        public int UnlockStage;
        public int HourlyIncome;

        public Sprite Icon;
        public GameObject Prefab;
    }


    public class GameBountyData
    {
        Dictionary<int, BountyData> data;

        public readonly float MaxUnclaimedHours;

        public GameBountyData(JSONNode node)
        {
            data = new Dictionary<int, BountyData>();

            LocalBountyData[] fromResources = LoadLocalData();

            MaxUnclaimedHours = node["maxUnclaimedHours"].AsFloat;

            foreach (LocalBountyData res in fromResources)
            {
                JSONNode current = node["bounties"][res.ID];

                data[res.ID] = new BountyData()
                {
                    Id = res.ID,
                    Name = res.Name,

                    Icon = res.Icon,
                    Prefab = res.Prefab,

                    UnlockStage = current["unlockStage"].AsInt,
                    HourlyIncome = current["hourlyIncome"].AsInt,
                };
            }
        }


        public BountyData Get(int key) => data[key];

        public int Count => data.Count;

        public bool GetStageBounty(int stage, out BountyData result)
        {
            result = default;

            foreach (KeyValuePair<int, BountyData> pair in data)
            {
                result = pair.Value;

                if (pair.Value.UnlockStage == stage)
                    return true;
            }

            return false;
        }

        public bool Contains(int id) => data.ContainsKey(id);

        LocalBountyData[] LoadLocalData() => Resources.LoadAll<LocalBountyData>("Bounties");
    }
}
