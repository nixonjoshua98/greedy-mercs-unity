using GM.Bounties.Models;
using GM.HTTP;
using System;

namespace GM.Bounties.Requests
{
    // Bounty Claim

    public class BountyClaimResponse : ServerResponse
    {
        public long PointsClaimed;

        public DateTime ClaimTime;

        public Inventory.UserCurrencies Currencies;
    }

    // Level Up Bounty

    public class UpgradeBountyRequest : IServerRequest
    {
        public int BountyID;
    }

    public class UpgradeBountyResponse : ServerResponse
    {
        public UserBounty Bounty;
    }
}
