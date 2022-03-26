using GM.HTTP;
using System;
using System.Collections.Generic;

namespace GM.Bounties.Models
{
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
