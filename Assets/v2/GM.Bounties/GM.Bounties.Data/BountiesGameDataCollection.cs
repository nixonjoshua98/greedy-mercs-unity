using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
