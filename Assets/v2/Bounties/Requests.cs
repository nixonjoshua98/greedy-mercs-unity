using GM.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GM.Bounties.Requests
{
    // Bounty Claim

    public class BountyClaimResponse : ServerResponse
    {
        public long PointsClaimed;

        public DateTime ClaimTime;

        public Inventory.UserCurrencies Currencies;
    }

    // Update Active Bounties

    public class SetActiveBountiesRequest : IServerRequest
    {
        public HashSet<int> BountyIds;

        public SetActiveBountiesRequest(List<int> bounties)
        {
            BountyIds = bounties.ToHashSet();
        }
    }
}
