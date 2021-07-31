using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;


namespace GM.Bounties
{
    using GM.Data;
    using GM.Server;

    public class BountyState
    {
        public readonly int ID;

        public DateTime LastClaimTime;

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


    public class UserBounties
    {
        Dictionary<int, BountyState> states;

        public List<BountyState> StatesList => states.Values.ToList();


        public UserBounties(JSONNode node)
        {
            states = new Dictionary<int, BountyState>();

            SetBounties(node);
        }


        // = = = Server Methods = = =
        public void ClaimPoints(Action action)
        {
            void Callback(long code, JSONNode resp)
            {
                if (code == 200)
                {
                    SetAllClaimTimes(Funcs.ToDateTime(resp["claimTime"].AsLong));
                    UserData.Get.Inventory.SetItems(resp["userItems"]);
                }

                action();
            }

            HTTPClient.GetClient().Post("bounty/claimpoints", Callback);
        }


        public BountySnapshot CreateSnapshot()
        {
            int capacity = 0;
            int unclaimed = 0;
            int hourlyIncome = 0;

            GameBountyData bountyGameData = GameData.Get.Bounties;

            // Calculate the attributes we want for the snapshot
            foreach (BountyState state in StatesList)
            {
                // Grab the static data for the struct
                BountyData dataStruct = bountyGameData.Get(state.ID);

                // We cap the hours since claim to the value returned from the server
                float hoursSinceClaim = Math.Max(0, Math.Min(bountyGameData.MaxUnclaimedHours, (float)(DateTime.UtcNow - state.LastClaimTime).TotalHours));

                capacity += Mathf.FloorToInt(dataStruct.HourlyIncome * bountyGameData.MaxUnclaimedHours);
                unclaimed += Mathf.FloorToInt(dataStruct.HourlyIncome * hoursSinceClaim);
                hourlyIncome += dataStruct.HourlyIncome;
            }

            // Create and return a new snapshot structure
            return new BountySnapshot
            {
                Capacity = capacity,
                Unclaimed = unclaimed,
                HourlyIncome = hourlyIncome,
                PercentFilled = unclaimed / (float)capacity // Cast to float to allow for a decimal answer
            };
        }


        void SetAllClaimTimes(DateTime date)
        {
            foreach (BountyState state in StatesList)
            {
                state.LastClaimTime = date;
            }
        }


        void SetBounties(JSONNode node)
        {
            foreach (JSONNode bounty in node.AsArray)
            {
                int id = bounty["bountyId"].AsInt;

                // Ignore 'disabled' bounties which are not in the server data
                if (GameData.Get.Bounties.Contains(id))
                {
                    states[id] = new BountyState(id)
                    {
                        LastClaimTime = Funcs.ToDateTime(bounty["lastClaimTime"].AsLong)
                    };
                }
                else
                {
                    Debug.LogWarning(string.Format("Bounty {0} is currently not available", id));
                }
            }
        }
    }
}