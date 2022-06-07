using GM.Bounties.Models;
using GM.HTTP;
using System;

namespace GM.Bounties.Requests
{
    public class BountyClaimResponse : ServerResponse
    {
        public long PointsClaimed;

        public DateTime ClaimTime;

        public Inventory.UserCurrencies Currencies;
    }

    public class UpgradeBountyResponse : ServerResponse
    {
        public UserBounty Bounty;
    }
}
