using GM.HTTP.Requests;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GM.Bounties.Data
{
    public class BountiesData : Core.GMClass
    {
        public BountiesGameDataCollection Game;
        public BountiesUserDataCollection User;

        public BountiesData(Models.CompleteBountyDataModel userData, Models.AllBountyGameDataModel gameData)
        {
            Game = new BountiesGameDataCollection(gameData);
            User = new BountiesUserDataCollection(userData);
        }

        public BountySnapshot CreateSnapshot()
        {
            int capacity = 0;
            int unclaimed = 0;
            int hourlyIncome = 0;

            // Calculate the attributes we want for the snapshot
            foreach (var state in User.States)
            {
                if (state.IsActive)
                {
                    // Grab the static data for the struct
                    Bounties.Models.BountyGameData dataStruct = Game.Get(state.BountyId);

                    // We cap the hours since claim to the value returned from the server
                    float hoursSinceClaim = Math.Max(0, Math.Min(Game.MaxUnclaimedHours, (float)(DateTime.UtcNow - User.LastClaimTime).TotalHours));

                    capacity += Mathf.FloorToInt(dataStruct.HourlyIncome * Game.MaxUnclaimedHours);
                    unclaimed += Mathf.FloorToInt(dataStruct.HourlyIncome * hoursSinceClaim);

                    hourlyIncome += dataStruct.HourlyIncome;
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
                    User.Update(resp.Bounties);
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
                    User.LastClaimTime = resp.ClaimTime;

                    App.Data.Inv.UpdateCurrencies(resp.CurrencyItems);
                }

                action(resp.StatusCode == 200, resp);
            });
        }
    }
}