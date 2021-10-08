using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Bounties.Data
{
    public class BountiesGameDataCollection : Dictionary<int, Models.BountyGameData>
    {
        public float MaxUnclaimedHours;
        public int MaxActiveBounties;

        public BountiesGameDataCollection(JSONNode node)
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

            foreach (var res in LoadLocalData())
            {
                JSONNode current = json[res.Id];

                base[res.Id] = new Models.BountyGameData()
                {
                    ID = res.Id,
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


        public bool GetStageBounty(int stage, out Models.BountyGameData result)
        {
            result = default;

            foreach (KeyValuePair<int, Models.BountyGameData> pair in this)
            {
                result = pair.Value;

                if (result.UnlockStage == stage)
                    return true;
            }

            return false;
        }

        static ScripableObjects.BountyLocalGameData[] LoadLocalData() => Resources.LoadAll<ScripableObjects.BountyLocalGameData>("Bounties");
    }
}
