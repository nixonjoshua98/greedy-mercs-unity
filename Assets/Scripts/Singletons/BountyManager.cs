using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using SimpleJSON;


namespace GM.Bounty
{
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

    public class BountyManager : MonoBehaviour
    {
        public static BountyManager Instance = null;

        Dictionary<int, BountyState> states;

        public static BountyManager Create(JSONNode node)
        {
            if (Instance != null)
            {
                Destroy(Instance.gameObject);
            }

            GameObject obj = new GameObject("BountyManager [Created]");

            Instance = obj.AddComponent<BountyManager>();

            Instance.Setup(node);

            return Instance;
        }

        void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        void Setup(JSONNode node)
        {
            states = new Dictionary<int, BountyState>();

            foreach (JSONNode bounty in node.AsArray)
            {
                int questId = bounty["questId"].AsInt;

                BountyState state = new BountyState(questId)
                {
                    LastClaimTime = Funcs.ToDateTime(bounty["lastClaimTime"].AsLong)
                };

                SetState(questId, state);
            }
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

                    GreedyMercs.GameState.Inventory.bountyPoints = returnData["totalBountyPoints"].AsInt;
                }

                call(code, body);
            }

            JSONNode postData = Funcs.GetDeviceInfo();

            Server.Put("bounty", "claimPoints", postData, Callback);
        }


        // = = = GET = = =
        public float PercentFilled { get { return UnclaimedTotal / (float)TotalCapacity; } }

        public List<BountyState> Bounties { get { return states.Values.ToList(); } }

        public int TotalHourlyIncome
        { 
            get
            {
                int total = 0;

                foreach (BountyState state in states.Values)
                {
                    int hourlyIncome = StaticData.BountyList.Get((GreedyMercs.BountyID)state.bountyId).hourlyIncome;

                    total += hourlyIncome;
                }

                return total;
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
                    float hoursSinceClaim = (float)(now - state.LastClaimTime).TotalHours;

                    int hourlyIncome = StaticData.BountyList.Get((GreedyMercs.BountyID)state.bountyId).hourlyIncome;

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
                    int hourlyIncome = StaticData.BountyList.Get(state.bountyId).hourlyIncome;

                    capacity += hourlyIncome * 6;
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