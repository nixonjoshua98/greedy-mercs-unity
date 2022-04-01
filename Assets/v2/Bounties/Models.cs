using System;
using System.Collections.Generic;

namespace GM.Bounties.Models
{
    public class UserBounty
    {
        public int BountyID;
        public int NumDefeats;
    }

    public class UserBounties
    {
        public DateTime LastClaimTime;
        public List<UserBounty> UnlockedBounties;
    }
}
