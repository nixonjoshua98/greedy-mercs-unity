using System.Collections.Generic;

namespace GM.Bounties.Models
{
    public struct CompleteBountyGameDataModel
    {
        public float MaxUnclaimedHours;
        public int MaxActiveBounties;

        public List<BountyGameData> Bounties;
    }
}