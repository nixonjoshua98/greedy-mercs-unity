using SRC.Bounties.Models;

namespace SRC.Bounties.Requests
{
    public class BountyClaimResponse : SRC.HTTP.Requests.ServerResponse
    {
        public long PointsClaimed;

        public Inventory.UserCurrencies Currencies;
    }

    public class UpgradeBountyResponse : SRC.HTTP.Requests.ServerResponse
    {
        public UserBounty Bounty;
    }
}
