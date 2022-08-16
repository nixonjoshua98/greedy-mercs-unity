using System.Collections.Generic;
using System;

namespace SRC.Bounties.Models
{
    public class UserBounties
    {
        public DateTime LastClaimTime;
        public List<UserBounty> UnlockedBounties;
    }

    public class UserBounty
    {
        public int BountyID;
        public int CurrentKillCount;
        public int Level;
    }
}
