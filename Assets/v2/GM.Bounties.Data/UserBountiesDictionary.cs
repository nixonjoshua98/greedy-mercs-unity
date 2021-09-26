using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;



namespace GM.Bounties.Data
{
    public class UserBountiesDictionary : Dictionary<int, UserBountyState>
    {
        public List<UserBountyState> UnlockedBounties => Values.ToList();

        public DateTime LastClaimTime;


        public UserBountiesDictionary(JSONNode json)
        {
            UpdateWithJSON(json);
        }


        public void UpdateWithJSON(JSONNode json)
        {
            LastClaimTime = Utils.UnixToDateTime(json["lastClaimTime"].AsLong);

            if (json.TryGetKey("bounties", out JSONNode result))
            {
                UpdateBountiesWithJSON(result);
            }
        }


        public void UpdateBountiesWithJSON(JSONNode json)
        {
            Clear();

            foreach (string key in json.Keys)
            {
                JSONNode current = json[key];

                int id = int.Parse(key);

                base[id] = new UserBountyState(id)
                {
                    IsActive = current.GetValueOrDefault("isActive", false).AsBool,
                };
            }
        }
    }
}