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
        public List<UnlockedBountyData> UserBountiesList => UserData.Bounties.Select(ele => GetUnlockedBounty(ele.BountyId)).ToList();

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
        Dictionary<int, BountyLocalGameData> LoadLocalData() => Resources.LoadAll<BountyLocalGameData>("Bounties").ToDictionary(ele => ele.Id, ele => ele);

        /// <summary>
        /// Fetch the bounty user data
        /// </summary>
        Models.BountyUserDataModel _GetUserBountyData(int key)
        {
            var data = UserData.Bounties.Where(ele => ele.BountyId == key).FirstOrDefault();

            Core.GMLog.LogIfNull(data, $"User bounty '{key}' is null");

            return data;
        }

        /// <summary>
        /// Fetch data for an unlocked bounty
        /// </summary>
        public UnlockedBountyData GetUnlockedBounty(int key) => new UnlockedBountyData(GetGameBounty(key), _GetUserBountyData(key));

        /// <summary>
        /// Forward the 'MaxActiveBounties' attribute from the game data
        /// </summary>
        public int MaxActiveBounties => GameData.MaxActiveBounties;

        /// <summary>
        /// Get data for a single bounty
        /// </summary>
        public Models.BountyGameData GetGameBounty(int key) => GameData.Bounties.Where(ele => ele.Id == key).FirstOrDefault();

        // Update the complete bounty data model
        void Update(Models.CompleteBountyGameDataModel data)
        {
            GameData = data;

            var allLocalBounties = LoadLocalData();

            foreach (var bounty in GameData.Bounties)
            {
                var localBounty = allLocalBounties[bounty.Id];

                bounty.Name = localBounty.Name;
                bounty.Icon = localBounty.Icon;
                bounty.Slot = localBounty.Slot;

                bounty.Prefab = localBounty.Prefab;
            }
        }


        public bool GetStageBounty(int stage, out Models.BountyGameData result)
        {
            result = default;

            foreach (Models.BountyGameData bounty in GameData.Bounties)
            {
                if (bounty.UnlockStage == stage)
                    return true;
            }

            return false;
        }

        public BountySnapshot CreateSnapshot()
        {
            int capacity = 0;
            int unclaimed = 0;
            int hourlyIncome = 0;

            // Calculate the attributes we want for the snapshot
            foreach (var state in UserData.Bounties)
            {
                if (state.IsActive)
                {
                    // Grab the static data for the struct
                    var bountyGameData = GetGameBounty(state.BountyId);

                    // We cap the hours since claim to the value returned from the server
                    float hoursSinceClaim = Math.Max(0, Math.Min(GameData.MaxUnclaimedHours, (float)(DateTime.UtcNow - UserData.LastClaimTime).TotalHours));

                    capacity += Mathf.FloorToInt(bountyGameData.HourlyIncome * GameData.MaxUnclaimedHours);
                    unclaimed += Mathf.FloorToInt(bountyGameData.HourlyIncome * hoursSinceClaim);

                    hourlyIncome += bountyGameData.HourlyIncome;
                }
            }

            // Create and return a new snapshot structure
            return new BountySnapshot
            {
                Capacity = capacity,
                Unclaimed = unclaimed,
                HourlyIncome = hourlyIncome,
                PercentFilled = capacity > 0 ? unclaimed / (float)capacity : 0 // Cast to float to allow for a decimal answer
            };
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
                }

                action(resp.StatusCode == 200, resp);
            });
        }
    }
}