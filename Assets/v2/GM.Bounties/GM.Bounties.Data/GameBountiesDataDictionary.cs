using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Bounties.Data
{
    public class GameBountiesDataDictionary : Dictionary<int, GameBountyData>
    {
        public float MaxUnclaimedHours;
        public int MaxActiveBounties;

        public GameBountiesDataDictionary(JSONNode node)
        {
            UpdateWithJSON(node);
        }


        /// <summary>
        /// Update dictionary with a JSON object
        /// </summary>
        public void UpdateWithJSON(JSONNode json)
        {
            // {"maxUnclaimedHours": ?, "maxActiveBounties": ?, "bounties": []}

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

            foreach (LocalBountyData res in LoadLocalData())
            {
                JSONNode current = json[res.ID];

                base[res.ID] = new GameBountyData()
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


        public bool GetStageBounty(int stage, out GameBountyData result)
        {
            result = default;

            foreach (KeyValuePair<int, GameBountyData> pair in this)
            {
                result = pair.Value;

                if (result.UnlockStage == stage)
                    return true;
            }

            return false;
        }

        static LocalBountyData[] LoadLocalData() => Resources.LoadAll<LocalBountyData>("Bounties");
    }
}
