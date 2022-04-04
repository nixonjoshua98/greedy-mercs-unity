using GM.Bounties.Requests;
using GM.Bounties.ScripableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Bounties.Models
{
    public class BountiesDataContainer : Core.GMClass
    {
        private UserBounties UserData;
        private BountiesDataModel GameData;

        public void Set(UserBounties userData, BountiesDataModel gameData)
        {
            UpdateUserData(userData);
            UpdateStaticData(gameData);
        }

        /// <summary>
        /// Fetch the data for all unlocked bounties
        /// </summary>
        public List<AggregatedBounty> UnlockedBounties => UserData.UnlockedBounties.OrderBy(x => x.BountyID).Select(ele => GetUnlockedBounty(ele.BountyID)).ToList();

        /// <summary>
        /// Time since the last claim
        /// </summary>
        private TimeSpan TimeSinceClaim
        {
            get
            {
                double secs = (DateTime.UtcNow - UserData.LastClaimTime).TotalSeconds;

                return TimeSpan.FromSeconds(Math.Clamp(secs, 0, GameData.MaxUnclaimedHours * 3_600));
            }
        }


        /// <summary>
        /// Percentage
        /// </summary>
        public float ClaimPercentFilled => (float)TimeSinceClaim.TotalSeconds / (GameData.MaxUnclaimedHours * 3_600);

        /// <summary>
        /// Total unclaimed points ready to claim
        /// </summary>
        public long TotalUnclaimedPoints => (long)Math.Floor(TimeSinceClaim.TotalHours * TotalHourlyIncome);

        /// <summary>
        /// Update the complete user bounty data
        /// </summary>
        private void UpdateUserData(UserBounties data)
        {
            UserData = data;
        }

        /// <summary>
        /// Update a single bounty (delete and re-add the bounty)
        /// </summary>
        private void UpdateUserBounty(UserBounty bounty)
        {
            UserData.UnlockedBounties.RemoveAll(b => b.BountyID == bounty.BountyID);
            UserData.UnlockedBounties.Add(bounty);
        }

        /// <summary>
        /// Load local data stored as scriptable objects
        /// </summary>
        private Dictionary<int, BountyLocalGameData> LoadLocalData()
        {
            return Resources.LoadAll<BountyLocalGameData>("Scriptables/Bounties").ToDictionary(ele => ele.Id, ele => ele);
        }

        /// <summary>
        /// Fetch the bounty user data
        /// </summary>
        private UserBounty GetUserBountyData(int key)
        {
            return UserData.UnlockedBounties.Where(ele => ele.BountyID == key).FirstOrDefault();
        }

        /// <summary>
        /// Calculate the total hourly income from all active bounties
        /// </summary>
        public long TotalHourlyIncome => UnlockedBounties.Sum(ele => ele.Income);

        /// <summary>
        /// Maximum points which can be claimed at once
        /// </summary>
        public long MaxClaimPoints => Mathf.FloorToInt(GameData.MaxUnclaimedHours * TotalHourlyIncome);

        /// <summary>
        /// Fetch data for an unlocked bounty
        /// </summary>
        public AggregatedBounty GetUnlockedBounty(int key)
        {
            return new AggregatedBounty(GetGameBounty(key), GetUserBountyData(key));
        }

        /// <summary>
        /// Get data for a single bounty
        /// </summary>
        public Models.Bounty GetGameBounty(int key)
        {
            return GameData.Bounties.Where(ele => ele.ID == key).FirstOrDefault();
        }

        /// <summary>
        /// Update the complete bounty data model
        /// </summary>
        private void UpdateStaticData(BountiesDataModel data)
        {
            GameData = data;

            var allLocalBounties = LoadLocalData();

            foreach (var bounty in GameData.Bounties)
            {
                var localBounty = allLocalBounties[bounty.ID];

                bounty.Icon = localBounty.Icon;

                bounty.Prefab = localBounty.Prefab;
            }
        }

        public bool TryGetStageBounty(int stage, out Models.Bounty result)
        {
            result = default;

            foreach (Models.Bounty bounty in GameData.Bounties)
            {
                if (bounty.UnlockStage == stage)
                {
                    result = bounty;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Send the request to claim the current unclaimed points
        /// </summary>
        public void ClaimPoints(UnityAction<bool, BountyClaimResponse> action)
        {
            App.HTTP.ClaimBounties((resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    UserData.LastClaimTime = resp.ClaimTime;

                    App.Inventory.UpdateCurrencies(resp.Currencies);

                    App.Events.BountyPointsChanged.Invoke(resp.PointsClaimed);
                }

                action(resp.StatusCode == 200, resp);
            });
        }

        public void UpgradeBounty(int bountyId, UnityAction<bool, UpgradeBountyResponse> action)
        {
            App.HTTP.UpgradeBounty(bountyId, resp =>
            {
                bool success = resp.StatusCode == HTTP.HTTPCodes.Success;

                if (success)
                {
                    UpdateUserBounty(resp.Bounty);
                }

                action.Invoke(success, resp);
            });
        }
    }
}