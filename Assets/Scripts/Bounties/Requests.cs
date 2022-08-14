using GM.Bounties.Models;
using System;

namespace GM.Bounties.Requests
{
    public class BountyClaimResponse : GM.HTTP.Requests.ServerResponse
    {
        public long PointsClaimed;

        public DateTime ClaimTime;

        public Inventory.UserCurrencies Currencies;
    }

    public class UpgradeBountyResponse : GM.HTTP.Requests.ServerResponse
    {
        public UserBounty Bounty;
    }
}
