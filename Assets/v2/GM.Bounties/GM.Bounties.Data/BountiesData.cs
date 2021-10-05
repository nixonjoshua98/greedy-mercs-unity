using SimpleJSON;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

using GM.HTTP.Requests;

/*
    HIERACHY
    ========

    BountiesData -> BountySnapshot

        BountiesGameData
            BountyLocalGameData
            BountyGameData

        BountiesUserData
            BountyUserData (Server Model)
*/


namespace GM.Bounties.Data
{
    public class BountiesData : Core.GMClass
    {
        public BountiesGameDataCollection Game;
        public BountiesUserDataCollection User;

        public BountiesData(JSONNode userJSON, JSONNode gameJSON)
        {
            Game = new BountiesGameDataCollection(gameJSON);
            User = new BountiesUserDataCollection(userJSON);
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
                    BountyGameData dataStruct = Game[state.BountyId];

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

            App.HTTP.UpdateActiveBounties(req, (resp) => {

                if (resp.StatusCode == 200)
                {
                    User.Update(resp.Bounties);
                }

                action(resp.StatusCode == 200, resp);
            });
        }


        public void ClaimPoints(UnityAction<bool, BountyClaimResponse> action)
        {
            App.HTTP.ClaimBounties((resp) =>
            {
                if (resp.StatusCode == 200)
                {
                    User.LastClaimTime = resp.ClaimTime;

                    App.Data.Inv.UpdateCurrencies(resp.UserCurrencies);
                }

                action(resp.StatusCode == 200, resp);
            });
        }
    }
}