using SimpleJSON;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GM.Bounties.Data
{
    public class BountiesGameDataCollection
    {
        public float MaxUnclaimedHours;
        public int MaxActiveBounties;

        List<Models.BountyGameData> bounties;

        public BountiesGameDataCollection(Models.AllBountyGameDataModel data)
        {
            Update(data);
        }

        public Models.BountyGameData Get(int key)
        {
            return bounties.Where(ele => ele.Id == key).FirstOrDefault();
        }

        void Update(Models.AllBountyGameDataModel data)
        {
            MaxUnclaimedHours = data.MaxUnclaimedHours;
            MaxActiveBounties = data.MaxActiveBounties;

            bounties = data.Bounties;

            ScripableObjects.BountyLocalGameData[] allLocalBounties = LoadLocalData();

            foreach (var bounty in bounties)
            {
                var localMerc = allLocalBounties.Where(ele => ele.Id == bounty.Id).First();

                bounty.Name = localMerc.Name;
                bounty.Icon = localMerc.Icon;
                bounty.Slot = localMerc.Slot;

                bounty.Prefab = localMerc.Prefab;
            }
        }

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
            //Clear();

            //foreach (var res in LoadLocalData())
            //{
            //    JSONNode current = json[res.Id];

            //    base[res.Id] = new Models.BountyGameData()
            //    {
            //        Id = res.Id,
            //        Name = res.Name,

            //        Icon = res.Icon,
            //        Slot = res.Slot,
            //        Prefab = res.Prefab,

            //        SpawnChance = current.GetValueOrDefault("spawnChance", 1.0f).AsFloat,
            //        UnlockStage = current["unlockStage"].AsInt,
            //        HourlyIncome = current["hourlyIncome"].AsInt,
            //    };
            //}
        }


        public bool GetStageBounty(int stage, out Models.BountyGameData result)
        {
            result = default;

            foreach (Models.BountyGameData bounty in bounties)
            {
                if (bounty.UnlockStage == stage)
                    return true;
            }

            return false;
        }

        static ScripableObjects.BountyLocalGameData[] LoadLocalData() => Resources.LoadAll<ScripableObjects.BountyLocalGameData>("Bounties");
    }
}
