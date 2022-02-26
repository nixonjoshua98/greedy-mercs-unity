using GM.Bounties.ScripableObjects;
using GM.HTTP.Requests;
using System;
using GM.Bounties.Models;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Bounties.Data
{
    public class BountiesData : Core.GMClass
    {
        Models.CompleteBountyDataModel UserData;
        Models.CompleteBountyGameDataModel GameData;

        public BountiesData(Models.CompleteBountyDataModel userData, Models.CompleteBountyGameDataModel gameData)
        {
            UpdateUserData(userData);
            UpdateStaticData(gameData);
        }

        /// <summary>
        /// Fetch the data for all unlocked bounties
        /// </summary>
        public List<UnlockedBountyData> UnlockedBountiesList => UserData.Bounties.Select(ele => GetUnlockedBounty(ele.BountyId)).ToList();

        /// <summary>
        /// Get only the active bounties
        /// </summary>
        public List<UnlockedBountyData> ActiveBountiesList => UnlockedBountiesList.Where(ele => ele.IsActive).ToList();

        /// <summary>
        /// Time since the last claim
        /// </summary>
        TimeSpan TimeSinceClaim
        {
            get
            {
                TimeSpan ts = DateTime.UtcNow - UserData.LastClaimTime;

                if (ts.TotalHours > GameData.MaxUnclaimedHours)
                {
                    return new TimeSpan(0, 0, Mathf.FloorToInt(GameData.MaxUnclaimedHours * 3_600));
                } 
                else if (ts.TotalHours < 0)
                {
                    return new TimeSpan();
                }

                return ts;
            }
        }

        /// <summary>
        /// Time until the user has reached the cap for idle collection time
        /// </summary>
        public TimeSpan TimeUntilMaxUnclaimedHours => new TimeSpan(0, 0, Mathf.FloorToInt(GameData.MaxUnclaimedHours * 3_600)) - TimeSinceClaim;

        /// <summary>
        /// Percentage
        /// </summary>
        public float ClaimPercentFilled => (float)TimeSinceClaim.TotalSeconds / (GameData.MaxUnclaimedHours * 3_600);

        /// <summary>
        /// Total unclaimed points ready to claim
        /// </summary>
        public long TotalUnclaimedPoints => (long)(TimeSinceClaim.TotalHours * TotalHourlyIncome);

        /// <summary>
        /// Update the complete user bounty data
        /// </summary>
        void UpdateUserData(CompleteBountyDataModel data) => UserData = data;

        /// <summary>
        /// Update only the user bounties
        /// </summary>
        void Update(List<BountyUserDataModel> bounties) => UserData.Bounties = bounties;

        public void UpdateAllData(CompleteBountyDataModel userData, CompleteBountyGameDataModel staticData)
        {
            UpdateUserData(userData);
            UpdateStaticData(staticData);
        }

        /// <summary>
        /// Load local data stored as scriptable objects
        /// </summary>
        Dictionary<int, BountyLocalGameData> LoadLocalData() => Resources.LoadAll<BountyLocalGameData>("Scriptables/Bounties").ToDictionary(ele => ele.Id, ele => ele);

        /// <summary>
        /// Fetch the bounty user data
        /// </summary>
        BountyUserDataModel GetUserBountyData(int key) => UserData.Bounties.Where(ele => ele.BountyId == key).FirstOrDefault();

        /// <summary>
        /// Calculate the total hourly income from all active bounties
        /// </summary>
        public long TotalHourlyIncome => ActiveBountiesList.Sum(ele => ele.Income);

        /// <summary>
        /// Maximum points which can be claimed at once
        /// </summary>
        public long MaxClaimPoints => Mathf.FloorToInt(GameData.MaxUnclaimedHours * TotalHourlyIncome);

        /// <summary>
        /// Fetch data for an unlocked bounty
        /// </summary>
        public UnlockedBountyData GetUnlockedBounty(int key) => new UnlockedBountyData(GetGameBounty(key), GetUserBountyData(key));

        /// <summary>
        /// Forward the 'MaxActiveBounties' attribute from the game data
        /// </summary>
        public int MaxActiveBounties => GameData.MaxActiveBounties;

        /// <summary>
        /// Get data for a single bounty
        /// </summary>
        public Models.BountyGameData GetGameBounty(int key) => GameData.Bounties.Where(ele => ele.Id == key).FirstOrDefault();

        /// <summary>
        /// Update the complete bounty data model
        /// </summary>
        void UpdateStaticData(CompleteBountyGameDataModel data)
        {
            GameData = data;

            var allLocalBounties = LoadLocalData();

            foreach (var bounty in GameData.Bounties)
            {
                var localBounty = allLocalBounties[bounty.Id];

                bounty.Name = localBounty.Name;
                bounty.Icon = localBounty.Icon;

                bounty.Prefab = localBounty.Prefab;
            }
        }

        public bool TryGetStageBounty(int stage, out Models.BountyGameData result)
        {
            result = default;

            foreach (Models.BountyGameData bounty in GameData.Bounties)
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
        /// Send the request to update the active bounties
        /// </summary>
        public void SetActiveBounties(List<int> ids, UnityAction<bool, UpdateActiveBountiesResponse> action)
        {
            App.HTTP.SetActiveBounties(ids, (resp) => {

                if (resp.StatusCode == 200)
                {
                    Update(resp.Bounties);
                }

                action(resp.StatusCode == 200, resp);
            });
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

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);

                    App.Events.BountyPointsChanged.Invoke(resp.PointsClaimed);
                }

                action(resp.StatusCode == 200, resp);
            });
        }
    }
}