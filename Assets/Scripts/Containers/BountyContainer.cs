using System;

using System.Collections.Generic;

using SimpleJSON;

using UnityEngine;

namespace GreedyMercs
{
    public class BountyContainer
    {
        const int MAX_HOURS = 6;

        public DateTime lastClaimTime;

        Dictionary<BountyID, UpgradeState> bountyStates;

        public BountyContainer(JSONNode node)
        {
            bountyStates = new Dictionary<BountyID, UpgradeState>();

            lastClaimTime = DateTime.UtcNow;

            Update(node);
        }

        public void Update(JSONNode node)
        {
            lastClaimTime = node.HasKey("lastClaimTime") ? DateTimeOffset.FromUnixTimeMilliseconds(node["lastClaimTime"].AsLong).DateTime : lastClaimTime;

            if (node.HasKey("bountyLevels"))
            {
                node = node["bountyLevels"];

                bountyStates = new Dictionary<BountyID, UpgradeState>();

                foreach (string key in node.Keys)
                {
                    bountyStates[(BountyID)int.Parse(key)] = new UpgradeState { level = node[key].AsInt };
                }
            }
        }

        public JSONNode ToJson()
        {
            JSONNode node = new JSONObject();

            node.Add("lastClaimTime", lastClaimTime.ToUnixMilliseconds());

            return node;
        }
        
        // === Helper ===

        public Dictionary<BountyID, BountySO> Unlocked()
        {
            int stage = Mathf.Max(GameState.LifetimeStats.maxPrestigeStage, GameState.Stage.stage);

            Dictionary<BountyID, BountySO> unlocked = new Dictionary<BountyID, BountySO>();

            foreach (BountySO bounty in StaticData.BountyList.BountyList)
            {
                if (stage > bounty.unlockStage || bountyStates.ContainsKey(bounty.BountyID))
                {
                    unlocked.Add(bounty.BountyID, bounty);
                }
            }

            return unlocked;
        }

        public int GetPrestigeBountyLevels()
        {
            return -1;
            //int total = 0;

            //foreach (BountySO bounty in StaticData.BountyList.BountyList)
            //{
            //    UpgradeState state = GameState.Bounties.GetState(bounty.BountyID);

            //    if (GameState.LifetimeStats.maxPrestigeStage > bounty.unlockStage && GameState.Stage.stage > bounty.unlockStage && bounty.maxLevel > state.level)
            //        total++;
            //}

            //return total;
        }

        // === States ===

        public UpgradeState GetState(BountyID bounty)
        {
            if (bountyStates.TryGetValue(bounty, out UpgradeState state))
                return state;

            AddState(bounty, 1);

            return GetState(bounty);
        }

        public void AddState(BountyID bounty, int level)
        {
            bountyStates[bounty] = new UpgradeState { level = level };
        }

        // === Attributes ===

        public int TimeSinceClaim
        {
            get
            {
                if (HourlyIncome == 0)
                    lastClaimTime = DateTime.UtcNow;

                float secondsSinceClaim = (float)(DateTime.UtcNow - lastClaimTime).TotalSeconds;

                return Mathf.Max(0, Mathf.FloorToInt(Mathf.Min(MAX_HOURS * 3_600.0f, secondsSinceClaim)));
            }
        }

        public float PercentFilled { get { return TimeSinceClaim / (MAX_HOURS * 3_600.0f); } }

        public int CurrentClaimAmount { get { return Mathf.FloorToInt((TimeSinceClaim / 3_600.0f) * HourlyIncome); } }

        public int MaxClaimAmount { get { return HourlyIncome * MAX_HOURS; } }

        public int HourlyIncome
        {
            get
            {
                int total = 0;

                foreach (var bounty in Unlocked())
                    total += Formulas.CalcBountyHourlyIncome(bounty.Key);

                return total;
            }
        }
    }
}