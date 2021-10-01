using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Bounties.Data
{
    public class BountiesGameData : Dictionary<int, BountyGameData>
    {
        public float MaxUnclaimedHours;
        public int MaxActiveBounties;

        public BountiesGameData(JSONNode node)
        {
            UpdateWithJSON(node);
        }


        /// <summary>
        /// Update dictionary with a JSON object
        /// </summary>
        public void UpdateWithJSON(JSONNode json)
        {
            MaxUnclaimedHours = json["maxUnclaimedHours"];
            MaxActiveBounties = json["maxActiveBounties"];

            if (json.TryGetKey("bounties", out JSONNode result))
            {
                UpdateBountiesWithJSON(result);
            }
        }


        public void UpdateBountiesWithJSON(JSONNode json)
        {
            Clear();

            foreach (BountyLocalGameData res in LoadLocalData())
            {
                JSONNode current = json[res.ID];

                base[res.ID] = new BountyGameData()
                {
                    ID = res.ID,
                    Name = res.Name,

                    Icon = res.Icon,
                    Slot = res.Slot,
                    Prefab = res.Prefab,

                    SpawnChance = current.GetValueOrDefault("spawnChance", 1.0f).AsFloat,
                    UnlockStage = current["unlockStage"].AsInt,
                    HourlyIncome = current["hourlyIncome"].AsInt,
                };
            }
        }


        public bool GetStageBounty(int stage, out BountyGameData result)
        {
            result = default;

            foreach (KeyValuePair<int, BountyGameData> pair in this)
            {
                result = pair.Value;

                if (result.UnlockStage == stage)
                    return true;
            }

            return false;
        }

        static BountyLocalGameData[] LoadLocalData() => Resources.LoadAll<BountyLocalGameData>("Bounties");
    }
}
