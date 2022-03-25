using GM.HTTP;
using System;
using System.Collections.Generic;

namespace GM.Bounties.Models
{
    public class BountyClaimResponse : ServerResponse
    {
        public long PointsClaimed;

        public DateTime ClaimTime;

        public Inventory.Models.UserCurrencies Currencies;
    }

    public class UserBounty
    {
        public int BountyID;
    }

    public class UserBounties
    {
        public DateTime LastClaimTime;
        public List<int> ActiveBounties;
        public List<UserBounty> UnlockedBounties;
    }
}
