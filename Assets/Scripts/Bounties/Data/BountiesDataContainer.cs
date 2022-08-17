using SRC.Bounties.Requests;
using SRC.Bounties.Scriptables;
using SRC.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SRC.Bounties.Models
{
    [System.Serializable]
    public class BountiesDataContainer : Core.GMClass
    {
        [SerializeField] private BountiesLocalDataFile LocalDataFile;
        private readonly List<AggregatedBounty> AggregatedBounties = new();

        private UserBounties UserData;
        private BountiesDataFile GameData;

        public void UpdateStoredData(UserBounties userData, BountiesDataFile gameData)
        {
            AggregatedBounties.Clear();

            UserData = userData;
            GameData = gameData;

            GameData.Bounties.ForEach(b => GetBounty(b.BountyID));
        }

        /// <summary>
        /// Fetch the data for all unlocked bounties
        /// </summary>
        public List<AggregatedBounty> UnlockedBounties => UserData.UnlockedBounties.OrderBy(x => x.BountyID).Select(ele => GetBounty(ele.BountyID)).ToList();

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

        public int BountiesDefeatedSincePrestige => GameData.Bounties.Where(b => App.GameState.Stage > b.UnlockStage).Count();

        /// <summary>
        /// Update a single bounty (delete and re-add the bounty)
        /// </summary>
        private void UpdateUserBounty(UserBounty bounty)
        {
            UserData.UnlockedBounties.RemoveAll(b => b.BountyID == bounty.BountyID);
            UserData.UnlockedBounties.Add(bounty);
        }

        /// <summary>
        /// Calculate the total hourly income from all active bounties
        /// </summary>
        public long TotalPointsPerHour => UnlockedBounties.Sum(ele => ele.PointsPerHour);

        /// <summary>
        /// Maximum points which can be claimed at once
        /// </summary>
        public long MaxClaimPoints => Mathf.FloorToInt(GameData.MaxUnclaimedHours * TotalPointsPerHour);

        /// <summary>
        /// Fetch data for an unlocked bounty
        /// </summary>
        public AggregatedBounty GetBounty(int key)
        {
            return AggregatedBounties.GetOrCreate(b => b.BountyID == key, () =>
            {
                // Dynamic 'GET' functions for data
                Func<UserBounty> getUserBounty = () => UserData.UnlockedBounties.FirstOrDefault(ele => ele.BountyID == key);
                Func<Bounty> getBounty = () => GameData.Bounties.FirstOrDefault(ele => ele.BountyID == key);
                Func<BountiesLocalDataFileBounty> getLocalDataFile = () => LocalDataFile.Bounties.FirstOrDefault(b => b.BountyID == key);

                return new(key, getBounty, getLocalDataFile, getUserBounty);
            });
        }

        public AggregatedBounty GetBountyForStage(int stage)
        {
            return AggregatedBounties.FirstOrDefault(b => b.Stage == stage);
        }

        /// <summary>
        /// Send the request to claim the current unclaimed points
        /// </summary>
        public void ClaimPoints(UnityAction<bool, BountyClaimResponse> action)
        {
            App.HTTP.ClaimBounties((resp) =>
            {
                if (resp.StatusCode == HTTPCodes.Success)
                {
                    UserData.LastClaimTime = DateTime.UtcNow;

                    App.Inventory.UpdateCurrencies(resp.Currencies);

                    App.Inventory.BountyPointsChanged.Invoke(resp.PointsClaimed);
                }

                action(resp.StatusCode == HTTP.HTTPCodes.Success, resp);
            });
        }

        public void UpgradeBounty(int bountyId, UnityAction<UpgradeBountyResponse> action)
        {
            App.HTTP.UpgradeBounty(bountyId, resp =>
            {
                if (resp.StatusCode == HTTPCodes.Success)
                    UpdateUserBounty(resp.Bounty);

                action.Invoke(resp);
            });
        }
    }
}