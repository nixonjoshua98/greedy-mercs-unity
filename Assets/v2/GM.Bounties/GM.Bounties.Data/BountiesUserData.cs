using Newtonsoft.Json;
using SimpleJSON;
using System;
using System.Collections.Generic;

namespace GM.Bounties.Data
{
    static class UserDataKey
    {
        public const string LastClaimTime = "lastClaimTime";
        public const string Bounties = "bounties";
    }


    public class BountiesUserData
    {
        public List<Models.BountyUserData> States = new List<Models.BountyUserData>();

        public DateTime LastClaimTime;

        public BountiesUserData(JSONNode json)
        {
            UpdateWithJSON(json);
        }


        public void UpdateWithJSON(JSONNode json)
        {
            LastClaimTime = Utils.UnixToDateTime(json[UserDataKey.LastClaimTime].AsLong);

            if (json.TryGetKey(UserDataKey.Bounties, out JSONNode result))
            {
                UpdateBountiesWithJSON(result);
            }
        }


        public void UpdateBountiesWithJSON(JSONNode json)
        {
            States = JsonConvert.DeserializeObject<List<Models.BountyUserData>>(json.ToString());
        }


        public void UpdateBounties(List<Models.BountyUserData> bounties)
        {
            States = bounties;
        }
    }
}