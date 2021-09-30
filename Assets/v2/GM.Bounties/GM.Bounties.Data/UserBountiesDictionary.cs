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


    public class UserBountiesDictionary
    {
        public List<UserBountyState> States = new List<UserBountyState>();

        public DateTime LastClaimTime;

        public UserBountiesDictionary(JSONNode json)
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
            States = JsonConvert.DeserializeObject<List<UserBountyState>>(json.ToString());
        }


        public void UpdateWithModel(List<UserBountyState> bounties)
        {
            States = bounties;
        }
    }
}