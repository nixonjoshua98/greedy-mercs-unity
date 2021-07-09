using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;


namespace GM.Bounty
{
    using GM.Inventory;

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


    public class BountyManager
    {
        Dictionary<int, BountyState> states;

        public List<BountyState> StatesList { get { return states.Values.ToList(); } }


        public BountyManager(JSONNode node)
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
                    long ts = resp["claimTime"].AsLong;

                    SetAllClaimTimes(Funcs.ToDateTime(ts));

                    InventoryManager.Instance.SetItems(resp["userItems"]);
                }

                action();
            }

            Server.Post("bounty/claimpoints", Callback);
        }


        public BountySnapshot CreateSnapshot()
        {
            int capacity = 0;
            int unclaimed = 0;
            int hourlyIncome = 0;

            // Calculate the attributes we want for the snapshot
            foreach (BountyState state in StatesList)
            {
                // Grab the static data for the struct
                BountyDataStruct dataStruct = StaticData.Bounty.Get(state.ID);

                // We cap the hours since claim to the value returned from the server
                float hoursSinceClaim = Math.Min(StaticData.Bounty.MaxUnclaimedHours, (float)(DateTime.UtcNow - state.LastClaimTime).TotalHours);

                capacity += Mathf.FloorToInt(dataStruct.HourlyIncome * StaticData.Bounty.MaxUnclaimedHours);
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
                if (StaticData.Bounty.Contains(id))
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