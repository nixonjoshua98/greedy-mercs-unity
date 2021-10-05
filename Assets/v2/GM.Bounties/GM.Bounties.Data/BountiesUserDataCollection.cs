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


    public class BountiesUserDataCollection
    {
        public List<Models.SingleBountyUserDataModel> States = new List<Models.SingleBountyUserDataModel>();

        public DateTime LastClaimTime;

        public BountiesUserDataCollection(JSONNode json)
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
            States = JsonConvert.DeserializeObject<List<Models.SingleBountyUserDataModel>>(json.ToString());
        }


        public void Update(List<Models.SingleBountyUserDataModel> bounties)
        {
            States = bounties;
        }
    }
}