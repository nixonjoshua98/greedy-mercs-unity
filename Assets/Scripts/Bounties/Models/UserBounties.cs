using System;
using System.Collections.Generic;

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
