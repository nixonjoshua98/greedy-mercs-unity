using SimpleJSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using GM.Data;
using GM.HTTP;
using GM.Bounties.Data;



namespace GM.Bounties
{
    public class BountyState
    {
        public readonly int ID;

        public bool IsActive;

        public BountyState(int bountyId)
        {
            ID = bountyId;
        }
    }

    public struct BountySnapshot
    {
        public int Capacity;
        public int Unclaimed;
        public int HourlyIncome;

        public float PercentFilled;
    }


    public class UserBounties : Core.GMClass
    {
        Dictionary<int, BountyState> states;
        public List<BountyState> StatesList => states.Values.ToList();
        public DateTime LastClaimTime;


        public UserBounties(JSONNode node)
        {

            LastClaimTime = Utils.UnixToDateTime(node["lastClaimTime"].AsLong);

            SetBounties(node["bounties"]);
        }


        // = = = Server Methods = = =
        public void ClaimPoints(Action<bool, long> action)
        {
            void Callback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    SetAllClaimTimes(Utils.UnixToDateTime(resp["claimTime"].AsLong));

                    App.Data.Inv.UpdateWithJSON(resp["userItems"]);
                }

                action(code == 200, code == 200 ? resp["pointsClaimed"].AsLong : -1);
            }

            HTTPClient.Instance.Post("bounty/claim", Callback);
        }


        public void SetActiveBounties(List<int> ids, Action<bool> action)
        {
            void Callback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    SetBounties(resp["userBounties"]);
                }

                action(code == 200);
            }

            JSONNode body = new JSONObject();
            body.AddList("bountyIds", ids);

            HTTPClient.Instance.Post("bounty/setactive", body, Callback);
        }


        public BountySnapshot CreateSnapshot()
        {
            int capacity = 0;
            int unclaimed = 0;
            int hourlyIncome = 0;

            GameBountiesDataDictionary bountyGameData = App.Data.Bounties.Game;

            // Calculate the attributes we want for the snapshot
            foreach (BountyState state in StatesList)
            {
                if (state.IsActive)
                {
                    // Grab the static data for the struct
                    GameBountyData dataStruct = bountyGameData[state.ID];

                    // We cap the hours since claim to the value returned from the server
                    float hoursSinceClaim = Math.Max(0, Math.Min(bountyGameData.MaxUnclaimedHours, (float)(DateTime.UtcNow - LastClaimTime).TotalHours));

                    capacity += Mathf.FloorToInt(dataStruct.HourlyIncome * bountyGameData.MaxUnclaimedHours);
                    unclaimed += Mathf.FloorToInt(dataStruct.HourlyIncome * hoursSinceClaim);
                    hourlyIncome += dataStruct.HourlyIncome;
                }
            }

            // Create and return a new snapshot structure
            return new BountySnapshot
            {
                Capacity = capacity,
                Unclaimed = unclaimed,
                HourlyIncome = hourlyIncome,
                PercentFilled = capacity > 0 ? unclaimed / (float)capacity : 0 // Cast to float to allow for a decimal answer
            };
        }


        void SetAllClaimTimes(DateTime date)
        {
            LastClaimTime = date;
        }


        void SetBounties(JSONNode node)
        {
            states = new Dictionary<int, BountyState>();

            foreach (string key in node.Keys)
            {
                JSONNode current = node[key];

                int id = int.Parse(key);

                states[id] = new BountyState(id)
                {
                    IsActive = current.GetValueOrDefault("isActive", false).AsBool,
                };
            }
        }
    }
}