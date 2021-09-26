using SimpleJSON;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GM.Bounties.Data
{
    public class BountiesData : Core.GMClass
    {
        public GameBountiesDataDictionary Game;
        public UserBountiesDictionary User;

        public BountiesData(JSONNode userJSON, JSONNode gameJSON)
        {
            Game = new GameBountiesDataDictionary(gameJSON);
            User = new UserBountiesDictionary(userJSON);
        }

        public BountySnapshot CreateSnapshot()
        {
            int capacity = 0;
            int unclaimed = 0;
            int hourlyIncome = 0;

            // Calculate the attributes we want for the snapshot
            foreach (UserBountyState state in User.UnlockedBounties)
            {
                if (state.IsActive)
                {
                    // Grab the static data for the struct
                    GameBountyData dataStruct = Game[state.ID];

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

        public void SetActiveBounties(List<int> ids, UnityAction<bool> action)
        {
            JSONNode body = new JSONObject();

            body.AddList("bountyIds", ids);

            App.HTTP.Post("bounty/setactive", body, (code, resp) => {

                if (code == 200)
                {
                    User.UpdateBountiesWithJSON(resp["userBounties"]);
                }

                action(code == 200);
            });
        }

        public void ClaimPoints(UnityAction<bool, long> action)
        {
            App.HTTP.Post("bounty/claim", (code, resp) =>
            {
                long pointsClaimed = code == 200 ? resp["pointsClaimed"].AsLong : 0;

                if (code == 200)
                {
                    User.LastClaimTime = Utils.UnixToDateTime(resp["claimTime"].AsLong);

                    App.Data.Inv.UpdateCurrenciesWithJSON(resp["userItems"]);
                }

                action(code == 200, code == 200 ? pointsClaimed : -1);
            });
        }
    }
}
