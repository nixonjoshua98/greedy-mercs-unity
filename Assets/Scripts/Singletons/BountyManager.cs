using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;


namespace GM.Bounty
{
    using GM.Inventory;

    using Utils = GM.Utils;

    public class BountyState
    {
        public readonly int ID;

        public DateTime LastClaimTime;

        public BountyState(int questId)
        {
            ID = questId;
        }
    }

    public class BountyManager
    {
        public static BountyManager Instance = null;

        Dictionary<int, BountyState> states;

        public BountyManager(JSONNode node)
        {
            states = new Dictionary<int, BountyState>();

            SetBounties(node);
        }

        public static BountyManager Create(JSONNode node)
        {
            Instance = new BountyManager(node);

            return Instance;
        }

        // = = = Server Methods = = =
        public void ClaimPoints(Action<long, string> call)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode resp = JSON.Parse(body);

                    SetAllClaimTimes(Funcs.ToDateTime(resp["claimTime"].AsLong));

                    InventoryManager.Instance.SetItems(resp["inventoryItems"]);
                }

                call(code, body);
            }

            Server.Put("bounty", "claimPoints", Callback);
        }


        // = = = GET = = =
        public float PercentFilled { get { return UnclaimedTotal / (float)TotalCapacity; } }

        public List<BountyState> Bounties { get { return states.Values.ToList(); } }

        public int MaxHourlyIncome
        { 
            get
            {
                int income = 0;

                foreach (BountyState state in states.Values)
                {
                    income += StaticData.Bounty.Get(state.ID).HourlyIncome;
                }

                return income;
            }
        }

        public int UnclaimedTotal
        {
            get
            {
                int unclaimed = 0;

                DateTime now = DateTime.UtcNow;

                foreach (BountyState state in states.Values)
                {
                    float hoursSinceClaim = Math.Min(StaticData.Bounty.MaxUnclaimedHours, (float)(now - state.LastClaimTime).TotalHours);

                    int hourlyIncome = StaticData.Bounty.Get(state.ID).HourlyIncome;

                    unclaimed += Mathf.FloorToInt(hoursSinceClaim * hourlyIncome);
                }

                return unclaimed;
            }
        }

        public int TotalCapacity
        {
            get
            {
                int capacity = 0;

                foreach (BountyState state in states.Values)
                {
                    int hourlyIncome = StaticData.Bounty.Get(state.ID).HourlyIncome;

                    capacity += Mathf.FloorToInt(hourlyIncome * StaticData.Bounty.MaxUnclaimedHours);
                }

                return capacity;
            }
        }


        // = = = SET = = =
        void SetAllClaimTimes(DateTime date)
        {
            foreach (BountyState state in states.Values)
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
                    BountyState state = new BountyState(id)
                    {
                        LastClaimTime = Funcs.ToDateTime(bounty["lastClaimTime"].AsLong)
                    };

                    states[id] = state;
                }
                else
                {
                    Debug.LogWarning(string.Format("Bounty {0} is currently not available", id));
                }
            }
        }
    }
}