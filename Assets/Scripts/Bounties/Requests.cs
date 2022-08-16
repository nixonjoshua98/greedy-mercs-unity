using SRC.Bounties.Models;
using System;

namespace SRC.Bounties.Requests
{
    public class BountyClaimResponse : SRC.HTTP.Requests.ServerResponse
    {
        public long PointsClaimed;

        public DateTime ClaimTime;

        public Inventory.UserCurrencies Currencies;
    }

    public class UpgradeBountyResponse : SRC.HTTP.Requests.ServerResponse
    {
        public UserBounty Bounty;
    }
}
