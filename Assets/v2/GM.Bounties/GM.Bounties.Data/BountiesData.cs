using GM.HTTP.Requests;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using GM.Bounties.ScripableObjects;

namespace GM.Bounties.Data
{
    public class BountiesData : Core.GMClass
    {
        Models.CompleteBountyDataModel UserData;
        Models.CompleteBountyGameDataModel GameData;

        public BountiesData(Models.CompleteBountyDataModel userData, Models.CompleteBountyGameDataModel gameData)
        {
            Update(userData);
            Update(gameData);
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

        public float ClaimPercentFilled => (float)TimeSinceClaim.TotalSeconds / (GameData.MaxUnclaimedHours * 3_600);

        /// <summary>
        /// Total unclaimed points ready to claim
        /// </summary>
        public long TotalUnclaimedPoints => (long)(TimeSinceClaim.TotalHours * TotalHourlyIncome);

        /// <summary>
        /// Update the complete user bounty data
        /// </summary>
        void Update(Models.CompleteBountyDataModel data) => UserData = data;

        /// <summary>
        /// Update only the user bounties
        /// </summary>
        void Update(List<Models.BountyUserDataModel> bounties) => UserData.Bounties = bounties;

        /// <summary>
        /// Load local data stored as scriptable objects
        /// </summary>
        Dictionary<int, BountyLocalGameData> LoadLocalData() => Resources.LoadAll<BountyLocalGameData>("Scriptables/Bounties").ToDictionary(ele => ele.Id, ele => ele);

        /// <summary>
        /// Fetch the bounty user data
        /// </summary>
        Models.BountyUserDataModel GetUserBountyData(int key) => UserData.Bounties.Where(ele => ele.BountyId == key).FirstOrDefault();

        /// <summary>
        /// Calculate the total hourly income from all active bounties
        /// </summary>
        public int TotalHourlyIncome => ActiveBountiesList.Sum(ele => ele.Income);

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
        void Update(Models.CompleteBountyGameDataModel data)
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


        public bool GetStageBounty(int stage, out Models.BountyGameData result)
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

        // === Server Methods === //

        public void SetActiveBounties(List<int> ids, UnityAction<bool, UpdateActiveBountiesResponse> action)
        {
            UpdateActiveBountiesRequest req = new UpdateActiveBountiesRequest() { BountyIds = ids };

            App.HTTP.Bounty_UpdateActives(req, (resp) => {

                if (resp.StatusCode == 200)
                {
                    Update(resp.Bounties);
                }

                action(resp.StatusCode == 200, resp);
            });
        }


        public void ClaimPoints(UnityAction<bool, BountyClaimResponse> action)
        {
            App.HTTP.Bounty_Claim((resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    UserData.LastClaimTime = resp.ClaimTime;

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);

                    App.Data.Inv.E_BountyPointsChange.Invoke(resp.PointsClaimed);
                }

                action(resp.StatusCode == 200, resp);
            });
        }
    }
}