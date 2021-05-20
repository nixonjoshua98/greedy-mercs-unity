using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;


namespace GM.Bounty
{
    using GM.Inventory;

    using StaticData = GreedyMercs.StaticData;

    using Utils = GreedyMercs.Utils;

    public class BountyState
    {
        public readonly int bountyId;

        public DateTime LastClaimTime;

        public BountyState(int questId)
        {
            bountyId = questId;
        }
    }

    public class BountyManager
    {
        public static BountyManager Instance = null;

        Dictionary<int, BountyState> states;

        public BountyManager()
        {
            states = new Dictionary<int, BountyState>();
        }

        public static BountyManager Create(JSONNode node)
        {
            Instance = new BountyManager();

            foreach (JSONNode bounty in node.AsArray)
            {
                int questId = bounty["bountyId"].AsInt;

                BountyState state = new BountyState(questId)
                {
                    LastClaimTime = Funcs.ToDateTime(bounty["lastClaimTime"].AsLong)
                };

                Instance.SetState(questId, state);
            }

            return Instance;
        }

        // = = = Server Methods = = =
        public void ClaimPoints(Action<long, string> call)
        {
            void Callback(long code, string body)
            {
                if (code == 200)
                {
                    JSONNode returnData = Utils.Json.Decompress(body);

                    SetAllClaimTimes(Funcs.ToDateTime(returnData["claimTime"].AsLong));

                    InventoryManager.Instance.BountyPoints = returnData["totalBountyPoints"].AsInt;
                }

                call(code, body);
            }

            JSONNode postData = Funcs.GetDeviceInfo();

            Server.Put("bounty", "claimPoints", postData, Callback);
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
                    income += StaticData.Bounty.Get(state.bountyId).HourlyIncome;
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

                    int hourlyIncome = StaticData.Bounty.Get(state.bountyId).HourlyIncome;

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
                    int hourlyIncome = StaticData.Bounty.Get(state.bountyId).HourlyIncome;

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

        void SetState(int questId, BountyState state)
        {
            states[questId] = state;
        }
    }
}